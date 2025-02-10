using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace RevitAddinBootcamp_TW1
{
    public class ExtractDeckTypeFromFloor
    {
        public static void ExtractDeckType(UIDocument uidoc)
        {
            Document doc = uidoc.Document;

            //Filter for all floor elements in the document
            FilteredElementCollector flrcoll = new FilteredElementCollector(doc);
            flrcoll.OfCategory(BuiltInCategory.OST_FloorsStructure);

            // Loop through each floor
            foreach (Floor floor in flrcoll)
            {
                // Check if the floor is a structural floor (Deck)
                if (floor.StructuralMaterialType == StructuralMaterialType.Concrete)
                {
                   
                }

            }
        }
    }
}
