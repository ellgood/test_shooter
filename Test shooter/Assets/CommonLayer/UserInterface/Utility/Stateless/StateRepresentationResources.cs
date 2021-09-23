namespace CommonLayer.UserInterface.Utility.Stateless
{
    public static class StateRepresentationResources
    {
        public static string MultipleTransitionsPermitted =>
            @"Multiple permitted exit transitions are configured from state '{1}' for trigger '{0}'. Guard clauses must be mutually exclusive.";
    }
}