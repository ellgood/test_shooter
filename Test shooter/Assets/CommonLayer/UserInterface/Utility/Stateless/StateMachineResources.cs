namespace CommonLayer.UserInterface.Utility.Stateless
{
    public static class StateMachineResources
    {
        public static string CannotReconfigureParameters => @"Parameters for the trigger '{0}' have already been configured.";

        public static string NoTransitionsPermitted =>
            @"No valid leaving transitions are permitted from state '{1}' for trigger '{0}'. Consider ignoring the trigger.";

        public static string NoTransitionsUnmetGuardConditions =>
            @"Trigger '{0}' is valid for transition from state '{1}' but a guard conditions are not met. Guard descriptions: '{2}'.";
    }
}