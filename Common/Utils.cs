namespace RevitAddinBootcamp_TW1.Common
{
    internal static class Utils
    {
        // cmdSkill03 methods (need to add 'static' to method so it can be used in any file in solution
        // and can doesn't need to be used before its defined, add 'using XXXX.Common;' to be beginning of .cs file
        // and add 'Utils.' before the method in body of the .cs files)

        public static void SetParameterValue(Element curElem, string paramName, string value)
        {
            Parameter curParam = curElem.LookupParameter(paramName);

            if (curParam != null)
            {
                curParam.Set(value);
            }
        }

        public static void SetParameterValue(Element curElem, string paramName, int value)
        {
            Parameter curParam = curElem.LookupParameter(paramName);

            if (curParam != null)
            {
                curParam.Set(value);
            }
        }

        public static string GetParameterValueAsString(Element curElem, string paramName)
        {
            Parameter curParam = curElem.LookupParameter(paramName);
            if (curParam != null)
            {
                return curParam.AsString();
            }
            else
                return "";
        }

        public static string GetParameterValueAsString(Element curElem, BuiltInParameter bip)
        // this is a overload method since the string name (GetParameterValueAsString) is used in two methods
        // because they have different arguements.
        {
            Parameter curParam = curElem.get_Parameter(bip);

            if (curParam != null)
            {
                return curParam.AsString();
            }
            else
                return "";
        }
        public static double GetParameterValueAsDouble(Element curElem, string paramName)
        {
            Parameter curParam = curElem.LookupParameter(paramName);
            if (curParam != null)
            {
                return curParam.AsDouble();
            }
            else
                return 0;
        }

        public static double GetParameterValueAsDouble(Element curElem, BuiltInParameter bip)
        {
            Parameter curParam = curElem.get_Parameter(bip);

            if (curParam != null)
            {
                return curParam.AsDouble();
            }
            else
                return 0;
        }


        public static FamilySymbol GetFamilySymbolByName(Document doc, string familyName, string familySymbolName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol curSymbol in collector)
            {
                if (curSymbol.FamilyName == familyName)
                {
                    if (curSymbol.Name == familySymbolName)
                    {
                        return curSymbol;
                    }
                }
            }

            return null;
        }










        // Ribbon methods
        internal static RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel curPanel;
            if (GetRibbonPanelByName(app, tabName, panelName) == null)
                curPanel = app.CreateRibbonPanel(tabName, panelName);
            else
                curPanel = GetRibbonPanelByName(app, tabName, panelName);
            return curPanel;
        }
        internal static RibbonPanel GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }
            return null;
        }
    }
}
