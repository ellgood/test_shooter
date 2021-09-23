namespace CommonLayer.UserInterface.Utility.Stateless
{
    public static class ParameterConversionResources
    {
        public static string ArgOfTypeRequiredInPosition => @"An argument of type {0} is required in position {1}.";
        public static string TooManyParameters => @"Too many parameters have been supplied. Expecting {0} but got {1}.";
        public static string WrongArgType => @"The argument in position {0} is of type {1} but must be of type {2}.";
    }
}