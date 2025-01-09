using System;
using System.Collections; 
using System.Collections.Generic;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Numerics;
public class BlipsCreator : Script {
    ScriptSettings IniSettings;
    /// /////////////////////RED
    /// </summary>
    Vector3 [] FireDepartment = new [] { new Vector3 (6134f, -3160f, 35f), 
                                        new Vector3 (6306f, -1527f, 10f), 
                                        new Vector3 (4915f, -1727f, 20f), 
                                        new Vector3 (5474f, -3614f, 4f), 
                                        new Vector3 (3031f, -3106f, 12f),};
    /// <summary>
    /// //////////////////////WHITE 
    /// </summary>
    Vector3 [] HospitalCoords = new [] { new Vector3 (6395f, -3058f, 34f), 
                                        new Vector3 (6433f, -2773f, 29f), 
                                        new Vector3 (1842f, 3668f, 30f), 
                                        new Vector3 (6169f, -1419f, 23f), 
                                        new Vector3 (4773f, -1936f, 17f),
                                        new Vector3 (5245f, -3065f, 14f),
                                        new Vector3 (4999f, -3819f, 5f),
                                        new Vector3 (3658f, -2884f, 21f),
                                        new Vector3 (3871f, -1978f, 22f) };
    Vector3 [] PoliceCoords = new [] { new Vector3 (6081.28f, -3616.94f, 18.99f), 
                                        new Vector3 (6418.89f, -3345.65f, 28.03f), 
                                        new Vector3 (7354f, -2806f, 7f), 
                                        new Vector3 (6425f, -2679f, 38f), 
                                        new Vector3 (6179f, -1381f, 24f), 
                                        new Vector3 (5623f, -1660f, 16f), 
                                        new Vector3 (5279f, -2043f, 14f), 
                                        new Vector3 (4771f, -2154f, 11f),
                                        new Vector3 (5274f, -2581f, 14f),
                                        new Vector3 (4782f, -2975f, 13f),
                                        new Vector3 (5003f, -3152f, 15f),
                                        new Vector3 (5400f, -3464f, 10f),
                                        new Vector3 (4808f, -3521f, 10f),
                                        new Vector3 (3964f, -3490f, 3f),
                                        new Vector3 (3471f, -2979f, 22f),
                                        new Vector3 (4264f, -1988f, 24f) };

    Vector3 [] AddBlipsCoords = new Vector3 [12];
    //////////////////////////
    int pos = 0, lenght, hud_colour = -1, hud1 = -1, hud2 = -2, Facility_Number = 0;
    float Xb = 100f, Yb = 100f;
    string Language = "it";
    bool GPS, MODActive = true, Facility = false, Police = true, Hospital = false, Fire = false, Persistence = false, GPSColour = true, GPSSprite = true, First = true, _blipReloaded = false;
    static bool [] AddBlip = new bool [12];
    static float [] X_Blip = new float [12];
    static float [] Y_Blip = new float [12];
    static float [] Z_Blip = new float [12];
    static int [] Blip_Sptite = new int [12];
    static int [] Blip_Colour = new int [12];
    static string [] Blip_Name = new string [12];
    static bool [] Facility_Active = new bool [9];
    static string [] Facility_Name = new string [9];
    static int [] Facility_Sprite = new int [9];
    static int [] Facility_Colour = new int [9];
    static float [] _X_Facility = new float [] { 1885f, 1271f, 2088f, 2754f, 1f, -2228f, -1f, 21f, 3387f };
    static float [] _Y_Facility = new float [] { 303f, 2833f, 1761f, 3904f, 2509f, 2399f, 3347f, 6824f, 5509f };
    static float [] _Z_Facility = new float [] { 30f, 30f, 30f, 30f, 30f, 30f, 30f, 30f, 30f };
    static string [] LanguageNameBlip = new string [25];
    static Blip [] AddedBlip = new Blip [12];
    static Blip [] Police_Blip = new Blip [16];
    static Blip [] Hospital_Blip = new Blip [9];
    static Blip [] Fire_Blip = new Blip [5];


    bool first = false;
    

    int number_blips = 12;
    List<Blip> _modBlips;
    static Blip FacilityBlip;
    public BlipsCreator () {
        IniSettings = ScriptSettings.Load ("scripts\\EmergencyBlips.ini");
        GPSColour = IniSettings.GetValue<bool> ("General", "GPS_Colour", true);
        GPSSprite = IniSettings.GetValue<bool> ("General", "GPS_Sprite", true);
        MODActive = IniSettings.GetValue<bool> ("General", "MODActive", true);
        Police = IniSettings.GetValue<bool> ("General", "Police_Station_Blips", true);
        Hospital = IniSettings.GetValue<bool> ("General", "Hospital_Blips", true);
        Fire = IniSettings.GetValue<bool> ("General", "Fire_Station_Blips", true);
        for (int k = 0; k < number_blips; k++){
            AddBlip [k] = IniSettings.GetValue<bool> ("Blip"+k, "Active", false);
            X_Blip [k] = IniSettings.GetValue ("Blip"+k, "X", 0f);
            Y_Blip [k] = IniSettings.GetValue ("Blip"+k, "Y", 0f);
            Z_Blip [k] = IniSettings.GetValue ("Blip"+k, "Z", 0f);
            Blip_Name [k] = IniSettings.GetValue<string> ("Blip"+k, "Custom_Name", "Blip"+k);
            Blip_Sptite [k] = IniSettings.GetValue<int> ("Blip"+k, "Custom_Sprite", 1);
            Blip_Colour [k] = IniSettings.GetValue<int> ("Blip"+k, "Custom_Colour", 1);
        }
        _modBlips = new List<Blip> ();
        GenerateBlips ();
        Tick += BlipsCreator_Tick;
    }

    private void BlipsCreator_Tick (object sender, EventArgs e) {
        GenerateBlips ();
        if (!_blipReloaded) {
            foreach (Blip bp in _modBlips) {
                if (!bp.Exists ()) {
                    GenerateBlips ();
                    _blipReloaded = true;
                    break;
                }
            }
        }
    }
    public bool is_present(Vector3 coords, int sprite){
        bool present = false, all_checked = false;
        Blip bl = Function.Call<Blip>(Hash.GET_FIRST_BLIP_INFO_ID,sprite);
        Vector3 start = Function.Call<Vector3>(Hash.GET_BLIP_INFO_ID_COORD,bl);
        while (!all_checked && Function.Call<bool>(Hash.DOES_BLIP_EXIST,bl)) {
            Vector3 coor = Function.Call<Vector3>(Hash.GET_BLIP_INFO_ID_COORD,bl);
            bool same = coords.Equals(coor);
            if(same) present = true;
            bl = Function.Call<Blip>(Hash.GET_NEXT_BLIP_INFO_ID,sprite);
            coor = Function.Call<Vector3>(Hash.GET_BLIP_INFO_ID_COORD,bl);
            all_checked = start.Equals(coor);
        }
        return present;
    }
    void NewBlip (Blip BlipName, Vector3 coords,  int sprite, int colour, string name, bool ShortRange) {
            int length = name.Length;
            bool b2 = string.IsNullOrEmpty(name);
            if(!(is_present(coords,sprite))){
                BlipName = World.CreateBlip(coords);
                Function.Call (Hash.SET_BLIP_SPRITE, BlipName, sprite);
                if (colour >= 0) Function.Call (Hash.SET_BLIP_COLOUR, BlipName, colour);
                if (!b2) { //String Length   
                    Function.Call (Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "STRING");
                    Function.Call (Hash._ADD_TEXT_COMPONENT_STRING, name);
                    Function.Call (Hash.END_TEXT_COMMAND_SET_BLIP_NAME, BlipName);
                }
                Function.Call (Hash.SET_BLIP_AS_SHORT_RANGE, BlipName, ShortRange);
                _modBlips.Add (BlipName);
            }
    }
    void LoadAddBlips () {
        for (pos = 0; pos < number_blips; pos++) {
            if (AddBlip[pos]) {
                AddBlipsCoords[pos] = new Vector3 ( (float)X_Blip[pos], (float)Y_Blip[pos], (float)Z_Blip[pos]);
                NewBlip (AddedBlip[pos],AddBlipsCoords [pos], Blip_Sptite [pos], Blip_Colour [pos], Blip_Name [pos], true);
            }
        }

    }
    private void GenerateBlips () {
        Player pl = Function.Call<Player>(Hash.GET_PLAYER_INDEX);
        _modBlips.Clear ();
        bool play = Function.Call<bool>(Hash.IS_PLAYER_PLAYING,pl);

        if (MODActive) {
            LoadAddBlips ();
            if (Fire) {
                for (int k = 0;k<Fire_Blip.Length;k++) {
                    NewBlip (Fire_Blip[k],FireDepartment[k], 436, 1, "", true);
                }
            };
            if (Hospital) {
                for (int k = 0;k<Hospital_Blip.Length;k++) {
                    NewBlip (Hospital_Blip[k], HospitalCoords[k],61, -1, "", true);
                }
            };
            if (Police) {
                for (int k = 0;k<Police_Blip.Length;k++) {
                    NewBlip (Police_Blip[k],PoliceCoords[k], 60, -1, "", true);
                }
            }  
        }
        Function.Call(Hash.SET_THIS_SCRIPT_CAN_REMOVE_BLIPS_CREATED_BY_ANY_SCRIPT, false);
    }
    public static Blip GetWaypointBlip () {
        if (!Game.IsWaypointActive) {
            return null;
        }

        for (int it = Function.Call<int> (Hash._GET_BLIP_INFO_ID_ITERATOR), blip = Function.Call<int> (Hash.GET_FIRST_BLIP_INFO_ID, it); Function.Call<bool> (Hash.DOES_BLIP_EXIST, blip); blip = Function.Call<int> (Hash.GET_NEXT_BLIP_INFO_ID, it)) {
            if (Function.Call<int> (Hash.GET_BLIP_INFO_ID_TYPE, blip) == 4) {
                return new Blip (blip);
            }
        }

        return null;
    }
}