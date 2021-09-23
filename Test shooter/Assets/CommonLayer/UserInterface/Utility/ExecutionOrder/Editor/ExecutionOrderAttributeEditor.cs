using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CommonLayer.UserInterface.Utility.ExecutionOrder.Editor
{
     public static class ExecutionOrderAttributeEditor
    {
        private static Dictionary<Type, MonoScript> GetTypeDictionary()
        {
            var types = new Dictionary<Type, MonoScript>();

            MonoScript[] scripts = MonoImporter.GetAllRuntimeMonoScripts();

            foreach (MonoScript script in scripts)
            {
                Type type = script.GetClass();
                if (!IsTypeValid(type))
                {
                    continue;
                }

                if (!types.ContainsKey(type))
                {
                    types.Add(type, script);
                }
            }

            return types;
        }

        private static bool IsTypeValid(Type type)
        {
            if (type != null)
            {
                return type.IsSubclassOf(typeof(MonoBehaviour)) || type.IsSubclassOf(typeof(ScriptableObject));
            }

            return false;
        }

        private static List<ScriptExecutionOrderDependency> GetExecutionOrderDependencies(
            IReadOnlyDictionary<Type, MonoScript> types)
        {
            var list = new List<ScriptExecutionOrderDependency>();

            foreach (KeyValuePair<Type, MonoScript> kvp in types)
            {
                Type type = kvp.Key;
                MonoScript script = kvp.Value;
                bool hasExecutionOrderAttribute = Attribute.IsDefined(type, typeof(ExecutionOrderAttribute));
                bool hasExecuteAfterAttribute = Attribute.IsDefined(type, typeof(ExecuteAfterAttribute));
                bool hasExecuteBeforeAttribute = Attribute.IsDefined(type, typeof(ExecuteBeforeAttribute));

                if (hasExecuteAfterAttribute)
                {
                    if (hasExecutionOrderAttribute)
                    {
                        Debug.LogError(
                            $"Script {script.name} has both [ExecutionOrder] and [ExecuteAfter] attributes. Ignoring the [ExecuteAfter] attribute.",
                            script);
                        continue;
                    }

                    var attributes =
                        (ExecuteAfterAttribute[]) Attribute.GetCustomAttributes(type, typeof(ExecuteAfterAttribute));
                    foreach (ExecuteAfterAttribute attribute in attributes)
                    {
                        if (attribute.OrderIncrease < 0)
                        {
                            Debug.LogError(
                                $"Script {script.name} has an [ExecuteAfter] attribute with a negative orderIncrease. Use the [ExecuteBefore] attribute instead. Ignoring this [ExecuteAfter] attribute.",
                                script);
                            continue;
                        }

                        if (!attribute.TargetType.IsSubclassOf(typeof(MonoBehaviour)) &&
                            !attribute.TargetType.IsSubclassOf(typeof(ScriptableObject)))
                        {
                            Debug.LogError(
                                $"Script {script.name} has an [ExecuteAfter] attribute with targetScript={attribute.TargetType.Name} which is not a MonoBehaviour nor a ScriptableObject. Ignoring this [ExecuteAfter] attribute.",
                                script);
                            continue;
                        }

                        MonoScript targetScript = types[attribute.TargetType];
                        var dependency = new ScriptExecutionOrderDependency
                            {FirstScript = targetScript, SecondScript = script, OrderDelta = attribute.OrderIncrease};
                        lock (list)
                        {
                            list.Add(dependency);
                        }
                    }
                }

                if (hasExecuteBeforeAttribute)
                {
                    if (hasExecutionOrderAttribute)
                    {
                        Debug.LogError(
                            $"Script {script.name} has both [ExecutionOrder] and [ExecuteBefore] attributes. Ignoring the [ExecuteBefore] attribute.",
                            script);
                        continue;
                    }

                    if (hasExecuteAfterAttribute)
                    {
                        Debug.LogError(
                            $"Script {script.name} has both [ExecuteAfter] and [ExecuteBefore] attributes. Ignoring the [ExecuteBefore] attribute.",
                            script);
                        continue;
                    }

                    var attributes =
                        (ExecuteBeforeAttribute[]) Attribute.GetCustomAttributes(type, typeof(ExecuteBeforeAttribute));
                    foreach (ExecuteBeforeAttribute attribute in attributes)
                    {
                        if (attribute.OrderDecrease < 0)
                        {
                            Debug.LogError(
                                $"Script {script.name} has an [ExecuteBefore] attribute with a negative orderDecrease. Use the [ExecuteAfter] attribute instead. Ignoring this [ExecuteBefore] attribute.",
                                script);
                            continue;
                        }

                        if (!attribute.TargetType.IsSubclassOf(typeof(MonoBehaviour)) &&
                            !attribute.TargetType.IsSubclassOf(typeof(ScriptableObject)))
                        {
                            Debug.LogError(
                                $"Script {script.name} has an [ExecuteBefore] attribute with targetScript={attribute.TargetType.Name} which is not a MonoBehaviour nor a ScriptableObject. Ignoring this [ExecuteBefore] attribute.",
                                script);
                            continue;
                        }

                        MonoScript targetScript = types[attribute.TargetType];
                        var dependency = new ScriptExecutionOrderDependency
                            {FirstScript = targetScript, SecondScript = script, OrderDelta = -attribute.OrderDecrease};
                        lock (list)
                        {
                            list.Add(dependency);
                        }
                    }
                }
            }

            return list;
        }

        private static List<ScriptExecutionOrderDefinition> GetExecutionOrderDefinitions(
            Dictionary<Type, MonoScript> types)
        {
            var list = new List<ScriptExecutionOrderDefinition>();

            foreach (KeyValuePair<Type, MonoScript> kvp in types)
            {
                Type type = kvp.Key;
                MonoScript script = kvp.Value;
                if (!Attribute.IsDefined(type, typeof(ExecutionOrderAttribute)))
                {
                    continue;
                }

                var attribute =
                    (ExecutionOrderAttribute) Attribute.GetCustomAttribute(type, typeof(ExecutionOrderAttribute));
                var definition = new ScriptExecutionOrderDefinition {Script = script, Order = attribute.Order};
                list.Add(definition);
            }

            return list;
        }

        private static Dictionary<MonoScript, int> GetInitialExecutionOrder(
            List<ScriptExecutionOrderDefinition> definitions, List<MonoScript> graphRoots)
        {
            var orders = new Dictionary<MonoScript, int>();
            foreach (ScriptExecutionOrderDefinition definition in definitions)
            {
                MonoScript script = definition.Script;
                int order = definition.Order;
                orders.Add(script, order);
            }

            foreach (MonoScript script in graphRoots)
            {
                if (!orders.ContainsKey(script))
                {
                    int order = MonoImporter.GetExecutionOrder(script);
                    orders.Add(script, order);
                }
            }

            return orders;
        }

        private static void UpdateExecutionOrder(Dictionary<MonoScript, int> orders)
        {
            var startedEdit = false;

            foreach (KeyValuePair<MonoScript, int> kvp in orders)
            {
                MonoScript script = kvp.Key;
                int order = kvp.Value;

                if (MonoImporter.GetExecutionOrder(script) == order)
                {
                    continue;
                }

                if (!startedEdit)
                {
                    AssetDatabase.StartAssetEditing();
                    startedEdit = true;
                }

                MonoImporter.SetExecutionOrder(script, order);
            }

            if (startedEdit)
            {
                AssetDatabase.StopAssetEditing();
            }
        }

        [DidReloadScripts]
        private static void OnDidReloadScripts()
        {
            Dictionary<Type, MonoScript> types = GetTypeDictionary();

            List<ScriptExecutionOrderDefinition> definitions = GetExecutionOrderDefinitions(types);
            List<ScriptExecutionOrderDependency> dependencies = GetExecutionOrderDependencies(types);

            Dictionary<MonoScript, List<Graph.Edge>> graph = Graph.Create(definitions, dependencies);
            if (Graph.IsCyclic(graph))
            {
                Debug.LogError("Circular script execution order definitions");
                return;
            }

            List<MonoScript> roots = Graph.GetRoots(graph);
            Dictionary<MonoScript, int> orders = GetInitialExecutionOrder(definitions, roots);
            Graph.PropagateValues(graph, orders);

            UpdateExecutionOrder(orders);
        }

        private static class Graph
        {
            public static Dictionary<MonoScript, List<Edge>> Create(List<ScriptExecutionOrderDefinition> definitions,
                List<ScriptExecutionOrderDependency> dependencies)
            {
                var graph = new Dictionary<MonoScript, List<Edge>>();

                foreach (ScriptExecutionOrderDependency dependency in dependencies)
                {
                    MonoScript source = dependency.FirstScript;
                    MonoScript dest = dependency.SecondScript;
                    if (!graph.TryGetValue(source, out List<Edge> edges))
                    {
                        edges = new List<Edge>();
                        graph.Add(source, edges);
                    }

                    edges.Add(new Edge {Node = dest, Weight = dependency.OrderDelta});
                    if (!graph.ContainsKey(dest))
                    {
                        graph.Add(dest, new List<Edge>());
                    }
                }

                foreach (ScriptExecutionOrderDefinition definition in definitions)
                {
                    MonoScript node = definition.Script;
                    if (!graph.ContainsKey(node))
                    {
                        graph.Add(node, new List<Edge>());
                    }
                }

                return graph;
            }

            public static bool IsCyclic(Dictionary<MonoScript, List<Edge>> graph)
            {
                var visited = new Dictionary<MonoScript, bool>();
                var inPath = new Dictionary<MonoScript, bool>();
                foreach (MonoScript node in graph.Keys)
                {
                    visited.Add(node, false);
                    inPath.Add(node, false);
                }

                foreach (MonoScript node in graph.Keys)
                {
                    if (IsCyclicRecursion(graph, node, visited, inPath))
                    {
                        return true;
                    }
                }

                return false;
            }

            public static List<MonoScript> GetRoots(Dictionary<MonoScript, List<Edge>> graph)
            {
                var degrees = new Dictionary<MonoScript, int>();
                foreach (MonoScript node in graph.Keys)
                {
                    degrees.Add(node, 0);
                }

                foreach (KeyValuePair<MonoScript, List<Edge>> kvp in graph)
                {
                    MonoScript node = kvp.Key;
                    List<Edge> edges = kvp.Value;
                    foreach (Edge edge in edges)
                    {
                        MonoScript succ = edge.Node;
                        degrees[succ]++;
                    }
                }

                var roots = new List<MonoScript>();
                foreach (KeyValuePair<MonoScript, int> kvp in degrees)
                {
                    MonoScript node = kvp.Key;
                    int degree = kvp.Value;
                    if (degree == 0)
                    {
                        roots.Add(node);
                    }
                }

                return roots;
            }

            public static void PropagateValues(IReadOnlyDictionary<MonoScript, List<Edge>> graph,
                Dictionary<MonoScript, int> values)
            {
                var queue = new Queue<MonoScript>();

                foreach (MonoScript node in values.Keys)
                {
                    queue.Enqueue(node);
                }

                while (queue.Count > 0)
                {
                    MonoScript node = queue.Dequeue();
                    int currentValue = values[node];

                    foreach (Edge edge in graph[node])
                    {
                        MonoScript succ = edge.Node;
                        int newValue = currentValue + edge.Weight;
                        bool hasPrevValue = values.TryGetValue(succ, out int prevValue);
                        bool newValueBeyond = edge.Weight > 0 ? newValue > prevValue : newValue < prevValue;
                        if (!hasPrevValue || newValueBeyond)
                        {
                            values[succ] = newValue;
                            queue.Enqueue(succ);
                        }
                    }
                }
            }

            private static bool IsCyclicRecursion(IReadOnlyDictionary<MonoScript, List<Edge>> graph, MonoScript node,
                IDictionary<MonoScript, bool> visited, IDictionary<MonoScript, bool> inPath)
            {
                if (!visited[node])
                {
                    visited[node] = true;
                    inPath[node] = true;

                    foreach (Edge edge in graph[node])
                    {
                        MonoScript succ = edge.Node;
                        if (IsCyclicRecursion(graph, succ, visited, inPath))
                        {
                            inPath[node] = false;
                            return true;
                        }
                    }

                    inPath[node] = false;
                    return false;
                }

                if (inPath[node])
                {
                    return true;
                }

                return false;
            }

            public struct Edge
            {
                public MonoScript Node;
                public int Weight;
            }
        }

        private struct ScriptExecutionOrderDefinition
        {
            public MonoScript Script { get; set; }
            public int Order { get; set; }
        }

        private struct ScriptExecutionOrderDependency
        {
            public MonoScript FirstScript { get; set; }
            public MonoScript SecondScript { get; set; }
            public int OrderDelta { get; set; }
        }
    }
}