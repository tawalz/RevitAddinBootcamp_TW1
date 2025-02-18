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
    [Transaction(TransactionMode.Manual)]
    public class steeldecktest : IExternalCommand    
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)        
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // 1. Filter for all floor elements in the document
            FilteredElementCollector flrColl = new FilteredElementCollector(doc);
            flrColl.OfCategory(BuiltInCategory.OST_FloorsStructure);


            using (Transaction t = new Transaction(doc))
            {
                t.Start("Identify steel decksin floor types");

                // 2. Loop through each floor
                 foreach (Floor floor in flrColl)
                 {
                                
                     // 3. Get Floor types and deck profile
                        FloorType flrType = floor.FloorType;

                        CompoundStructure flrStruc = flrType.GetCompoundStructure();
                        List<CompoundStructureLayer>flrLayers = flrStruc.GetLayers().ToList();


                        foreach (CompoundStructureLayer layer in flrLayers)
                        {
                            if (layer.DeckProfileId != null)
                            { 
                                FamilySymbol deckPro = doc.GetElement(layer.DeckProfileId) as FamilySymbol;
                                string proName = deckPro.Name;

                                Parameter parameter = flrType.LookupParameter("Comments");
                                //replace comments above with correct shared parameter name

                                parameter.Set(proName);
                            }
                
                        }

        


                 }


                t.Commit();
            }
           
            return Result.Succeeded;
        }
    }
}
