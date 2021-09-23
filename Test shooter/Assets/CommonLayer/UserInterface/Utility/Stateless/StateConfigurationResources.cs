namespace CommonLayer.UserInterface.Utility.Stateless
{
    public static class StateConfigurationResources
    {
        public static string SelfTransitionsEitherIgnoredOrReentrant =>
            @"Permit() (and PermitIf()) require that the destination state is not equal to the source state. To accept a trigger without changing state, use either Ignore() or PermitReentry()";
    }
}