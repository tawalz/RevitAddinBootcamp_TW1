using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RevitAddinBootcamp_TW1.Common;


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
            flrColl.OfClass(typeof(FloorType));


            using (Transaction t = new Transaction(doc, "Identify Steel Deck Profiles"))
            {
                t.Start();


                // 2. Loop through each floor
                int counter = 0;




                foreach (FloorType flrType in flrColl)
                {
                    

                    // 3. Get Floor types and deck profile

                    if (flrType != null)
                    {


                        /* Get the material name and set it to matTyp
                        List<Material> materialIds = flrType.GetMaterial().ToList();
                        {
                            foreach (Material mat in materialIds)
                            {
                                string matTyp = mat.Name;
                            
                            }
                        
                        
                        }
                        */



                        // Get the structural material ID
                        ElementId structuralMaterialId = flrType.StructuralMaterialId;
                        
                        Material structuralMaterial = doc.GetElement(structuralMaterialId) as Material;
                            
                        string matTyp = structuralMaterial.Name; 
                            
                        Parameter parameter4 = flrType.LookupParameter("Model");
                        
                        parameter4.Set(matTyp);


                        
                            







                        CompoundStructure flrStruc = flrType.GetCompoundStructure();
                        if (flrStruc != null)
                        {

                            List<CompoundStructureLayer> flrLayers = flrStruc.GetLayers().ToList();

                            double slabTk = flrStruc.GetWidth();

                            
                            



                            if (flrLayers != null)
                            {

                                foreach (CompoundStructureLayer layer in flrLayers)
                                {





                                    FamilySymbol deckPro = doc.GetElement(layer.DeckProfileId) as FamilySymbol;
                                    if (deckPro != null)
                                    {


                                        string proName = deckPro.Name;

                                        Parameter parameter = flrType.LookupParameter("Deck Type");
                                        //replace comments above with correct shared parameter name


                                        {
                                            parameter.Set(proName);


                                        }



                                        List<Parameter> deckTk = deckPro.GetParameters("hr").ToList();
                                        if (deckTk != null)
                                        {
                                            foreach (Parameter param in deckTk)
                                            {
                                                double deckThick = param.AsDouble();
                                                if (deckThick != 0)
                                                {
                                                    double slabThick = slabTk - deckThick;

                                                    Parameter parameter3 = flrType.LookupParameter("Deck Height");
                                                    
                                                    parameter3.Set(deckThick);

                                                    Parameter parameter2 = flrType.LookupParameter("Floor Slab Thickness");
                                                    
                                                    parameter2.Set(slabThick);


                                                    // Replace the line where slabInches is set with the following:
                                                    string slabInchesFraction = ConvertToFractionalInches(slabThick * 12);
                                                    
                                                    

                                                    if (slabThick > 0)
                                                    {
                                                        Parameter parameter1 = flrType.LookupParameter("Floor Slab Thick and Material");
                                                        parameter1.Set(slabInchesFraction + " " + matTyp);

                                                    }

                                                    counter++;
                                                    


                                                }
                                            }


                                        }

                                    }




                                }

                            }


                        }


                    }










                }

                TaskDialog.Show("Steel Deck Identification", $"{counter} Steel Deck Profiles have been identified in the floor types");


                t.Commit();
            }

            return Result.Succeeded;
        }
        public static string ConvertToFractionalInches(double decimalInches)
        {
            int wholeNumber = (int)decimalInches;
            double fractionalPart = decimalInches - wholeNumber;

            int numerator = (int)(fractionalPart * 64);
            int gcd = GCD(numerator, 64);
            numerator /= gcd;
            int denominator = 64 / gcd;

            if (numerator == 0)
            {
                return $"{wholeNumber}\"";
            }
            else if (wholeNumber == 0)
            {
                return $"{numerator}/{denominator}\"";
            }
            else
            {
                return $"{wholeNumber} {numerator}/{denominator}\"";
            }
        }

        private static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
