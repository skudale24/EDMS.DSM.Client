using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace EDM.Common
{
    public class CommonKeys
    {
        #region --- Find Contractor - CP ---
        public static String Contractor = "Contractor";

        #endregion

        #region --- Field Planner, Vacation Time Off & Relocation---
        public static String ACTIVE = "ACTIVE";
        public static String Yes = "Yes";
        public static String Multi = "Multi";
        public static String INACTIVE = "INACTIVE";
        public static String HOLIDAY = "HOLIDAY";
        public static String Region = "Region";
        #endregion

        public static String Error = "Error";
        public static String Longitude = "Longitude";
        public static String Latitude = "Latitude";
        public static String Successs = "Successs";
        public static String AddNotes = "AddNotes";

        #region --- Measure Names ---
        public static String AirSealing = "Air Sealing";
        public static String AtticInsulation = "Attic Insulation";
        public static String DuctSystem = "Duct System";
        public static String Lighting = "Lighting";
        public static String HVACSystem = "HVAC System";
        public static String AppliancesNElectronics = "Appliances/Electronics";
        public static String WaterHeating = "Water Heating";
        public static String Refrigerators = "Refrigerators";
        public static String WindowsNDoors = "Windows & Doors";
        public static String WallInsulation = "Wall Insulation";
        public static String DirectH2O = "Direct H2O";
        public static String HealthNSafety = "Health & Safety";
        #endregion

        #region ---Web API ---
        public static String scheme = "basic";
        public static String Encoding = "iso-8859-1";
        #endregion

        #region ---NEAT --- 
        #region ---Wall --- 
            public static String WallTypeCategory = "NWallLoadType";
            public static String WallStudSizeCategory = "NWallStudSize";
            public static String WallExteriorTypeCategory = "NWallExterior";
            public static String WallExposedToCategory = "NWallExposure";
            public static String OrientationCategory = "Norientation";
            public static String WallExistingInsulationTypeCategory = "NWallExistInsulation";
            public static String WallAddedInsulationTypeCategory = "NAddWallInsulation";
        #endregion

        #region ---Window --- 
            public static String WindowTypeCategory = "NWindowType";
            public static String WindowFrameTypeCategory = "NWindowFrameType";
            public static String WindowGlazingTypeCategory = "NWindowGlazingType";
            public static String WindowInteriorShadeCategory = "MWindowInteriorShade";
            public static String WindowLeakinessCategory = "WindowLeakiness";
            public static String WindowRetrofitStatusCategory = "NWindowRetrofitStatus";
            public static String WindowWallCode = "NWindowWallCode";
        
        #endregion

        #region ---Door --- 
        public static String DoorTypeCategory = "NDoorType";
        public static String DoorStormCondition = "NDoorStorm";
        public static String DoorLeakinessCategory = "Mleakiness";
        #endregion

        #region ---Heating --- 
        public static String HeatingEquipTypeCategory = "HeatingEquipType";
        public static String FuelTypeCategory = "FuelType";
        public static String HeatingEquipLocationCategory = "HeatingEquipLocation";
        public static String UnInsulatedDuctLocationCategory = "NUnInsulatedDuctLocation";
        public static String UnInsulatedDuctTypeCategory = "NUnInsulatedDuctType";
        public static String HeatingUnitsCategory = "HeatingUnits";
        public static String EquipConditionCategory = "EquipCondition";
        public static String HeatingUnitsElecCategory = "HeatingUnitsElec";
        public static String HeatingRetroStatusElecCategory = "HeatingRetroStatusElec";
        public static String VentDamperTypeCategory = "VentDamperType";
        public static String VentDamperConditionCategory = "VentDamperCondition";
        public static String VentChimneyTypeCategory = "VentChimneyType";
        public static String VentFlueTypeCategory = "VentFlueType";
        public static String VentCombustionSysTypeCategory = "VentCombustionSysType";
        public static String VentIntakeTypeCategory = "VentIntakeType";
        public static String BurnerTypeCategory = "BurnerType";
        public static String PilotTypeCategory = "PilotType";
        public static String BlowerTypeCategory = "BlowerType";
        public static String BlowerConditionCategory = "BlowerCondition";
        public static String FilterConditionCategory = "FilterCondition";
        public static String BoilerDistSystemTypeCategory = "BoilerDistSystemType";
        public static String BoilerDistPumpLocationCategory = "BoilerDistPumpLocation";
        public static String ConditionCategory = "Condition";
        public static String BoilerConvectorTypeCategory = "BoilerConvectorType";
        public static String ThermostatTypeCategory = "ThermostatType";
        
        #endregion

        #region ---Finished Attic --- 
        public static String FinAtticAreaType = "NFinAtticCode";
        public static String FinAtticFloorType = "NFinAtticType";
        public static String AtticRoofColor = "MCeilingColor";
        public static String AtticType = "NAtticExistInsl";
        public static String AtticTypeAdd = "NAddAtticInsulation";
        public static String AtticTypeAddKneewall = "NAddKneewallInsulation";
        #endregion

        #region ---UnFinished Attic --- 
        public static String UnFinAtticType = "NAtticType";
        #endregion

        #region ---Foundation --- 
        public static String FoundationTypeCategory = "NFoundationType";
        public static String AddFoundationInsulationCategory = "NAddFoundationInsulation";
        public static String AddSillInsulationCategory = "NAddSillInsulation";
        public static String AddFloorInsulationCategory = "NAddFloorInsulation";
        #endregion ---Foundation --- 

        #region ---Cooling --- 
        public static String NCoolingEquipType = "NCoolingEquipType";
        #endregion

        #region ---AirAndDuctLeakage --- 
        public static String NDuctLeakMethod = "NDuctLeakMethod";
        #endregion

        #region ---Wall --- 
        public static String RefrigStyle = "RefrigStyle";
        public static String RefrigDefrost = "RefrigDefrost";
        public static String HeatingEquipLocation = "HeatingEquipLocation";
        public static String RefrigDoorSealCondition = "RefrigDoorSealCondition";
        public static String RefrigAge = "RefrigAge";


        #endregion

        #region ---Blower Door --- 
        public static String ConductedDuringCategory = "BDPerformedDuring";
        
        #endregion

        #region ---Light --- 
        public static String LightLocation = "LightLocation";
        public static String LightType = "LightType";
        #endregion

        #region ---Water Heating --- 
        public static String FuelType = "NWaterHeatFuelType";
        public static String HeatingLocation = "HeatingEquipLocation";
        public static String InputUnitsType = "NWaterHeatInputUnitsType";
        public static String OrgTnkInsulationType = "NWaterHeatInsulType";
        public static String ReplacementFuel = "NWaterHeatFuelType";
        public static String ReplacementInputUnits = "NWaterHeatInputUnitsType";
        public static String DamperType = "VentDamperType";
        public static String DamperCondition = "VentDamperCondition";
        public static String ChimneyType = "VentChimneyType";
        public static String ChimneyEquipCondition = "EquipCondition";
        public static String FlueType = "VentFlueType";
        public static String FlueEquipCondition = "EquipCondition";
        public static String IntakeType = "VentIntakeType";
        public static String ElectricserviceSwitch = "EquipCondition";
        #endregion

         #region----Health & Safety
        public static String BDPerformedDuring = "BDPerformedDuring";

        #region ---Itemized Cost --- 
        public static String FuelTypeItemized = "FuelTypeItemized";
        public static String EnergySavingsUnits = "EnergySavingsUnits";

        #endregion
        #endregion
        #endregion

        #region ---MHEA --- 
        #region ---Audit Info --- 
            public static String WindShieldingCategory = "MWindShielding";
            public static String HomeLeakinessCategory = "Mleakiness";
        #endregion
        #region ---Wall  ---
        public static String MWallStudSizeCategory = "MWallStudSize";
        public static String WallVentingCategory = "MWallVenting";
        public static String WallConfigurationCategory = "MAdditionWallConfig";
        #endregion
        #region---Window---
        public static String MWindowGlazingType = "MWindowGlazingType";
        public static String MWindowType = "MWindowType";
        public static String WindowFrameType = "WindowFrameType";
        public static String MWindowInteriorShade = "MWindowInteriorShade";
        public static String WindowLeakiness = "WindowLeakiness";
        public static String MWindowExteriorShade = "MWindowExteriorShade";
        public static String MWindowRetrofitStatus = "MWindowRetrofitStatus";
        #endregion
        #region ---Heating --- 
        public static String MHeatingEquipTypeCategory = "MHeatingEquipType";
        public static String HeatingEfficiencyUnitsCategory = "HeatingEfficiencyUnits";
        public static String DuctLocationCategory = "MDuctLocation";
        public static String DuctInsulCategory = "MDuctInsul";
        public static String FilterLocationCategory = "MFilterLocation";
        #endregion
        #region ---Cooling --- 
        public static String MCoolingEquipTypeCategory = "MCoolingEquipType";
        public static String CoolingEfficiencyUnitsCategory = "CoolingEfficiencyUnits";
        #endregion
        #region----Ceiling---
        public static String MCeilingType = "MCeilingType";
        public static String MCeilingColor = "MCeilingColor";
        public static String MFloorJoistSize = "MFloorJoistSize";
        public static String MCeilingStepWallConfig = "MCeilingStepWallConfig";
        #endregion
        #region---Door---
        public static String MDoorType = "MDoorType";
        #endregion
        #region---Floor---
        public static String MFloorInsulLoc = "MFloorInsulLoc";
        public static String MFloorJoistDirection = "MFloorJoistDirection";
        public static String MFloorBellyConfig = "MFloorBellyConfig";
        public static String MFloorBellyCondition = "MFloorBellyCondition";
        public static String MFloorAddType = "MFloorAddType";
        #endregion
        #region ---AirAndDuctLeakage --- 
        public static String MDuctLeakMethod = "MDuctLeakMethod";
        #endregion
        #region ---Water Heating --- 
        public static String MChimneyType = "MVentChimneyType";
        #endregion

        #endregion

        /*Start 25 OCT 2018 |Swapnil Bhave| DSM-625: UV:14220-2: QCN Force Acknowledgement of Agreement*/
        public static String ShowTerms = "ShowTerms";
        public static String ShowNotification = "ShowNotification";
        public static String ShowSmallBusiness = "ShowSmallBusiness";
        public static String IsCheckUserTermsNotification = "IsCheckUserTermsNotification";
        /*End 25 OCT 2018 |Swapnil Bhave| DSM-625: UV:14220-2: QCN Force Acknowledgement of Agreement*/

        #region ---ProjectNoteType --- 
        public static int NotesInProgressInspection = 20;
        public static int NotesGeneralAppointment = 21;
        public static int NotesGeneralWorkOrderComments = 14;

        #endregion

        #region ---ProjectImageType --- 
        public static string ImageInProgressInspection = "IN PROGRESS INSPECTION";
        public static string ImageGeneralAppointment = "GENERAL APPOINTMENT";
        #endregion
        #region ---ProjectImageType --- 
        public static string AdvisorSurvey = "Advisor";
        public static string ContractorSurvey = "Contractor";
        public static string HealthSurvey = "Health";
        #endregion        
    }

    public class ThemeImageUrls {
        /// <summary>
        /// Property is used for the Required Field test which is path of the image file.
        /// Declared this as static so that We can use this throught syetem
        /// </summary>
        public static String Theme = String.Empty;
        public static String rfvImageUrlText = String.Empty;

        public static void Set(String themename)
        {
            if (String.IsNullOrEmpty(Theme))
                Theme = themename;
            if (String.IsNullOrEmpty(rfvImageUrlText))
                rfvImageUrlText = String.Format("<img src='/App_Themes/{0}/Images/error.gif' />", Theme);
        }
    }
}
