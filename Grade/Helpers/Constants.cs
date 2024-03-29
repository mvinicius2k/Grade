﻿namespace Grade.Helpers
{
    public static class Constants
    {

        public const int PageSize = 2;

        ///Rotas comuns
        public const string ControllerDefaultRoute = "[controller]";
        public const string GetAllActionRoute = "getAll";
        public const string GetActionRoute = "get";
        public const string CreateActionRoute = "create";
        public const string DetailsActionRoute = "details";
        public const string EditActionRoute = "edit";
        public const string DeleteActionRoute = "delete";

        //Matemática
        public const int MegaByte = 1024 * 1024;

        //Mime
        public static readonly string[] MimeImage = new string[] { "image/jpeg", "image/png", "image/webp" };
    }
}
