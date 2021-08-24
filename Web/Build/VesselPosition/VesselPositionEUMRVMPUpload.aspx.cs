using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using Telerik.Web.UI;

public partial class VesselPositionEUMRVMPUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Filter.CurrentMenuCodeSelection != null)
            SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Upload", "UPLOAD",ToolBarDirection.Right);
        AttachmentList.AccessRights = this.ViewState;
        AttachmentList.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ucVessel.bind();
            ucVessel.DataBind();

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
        }
    }
    protected void AttachmentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                if(General.GetNullableInteger(ucVessel.SelectedVessel)==null)
                {
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                    return;
                }

                String dtKey = string.Empty;
                dtKey = PhoenixCommonFileAttachment.GenerateDTKey();
                //HttpFileCollection postedFiles = Request.Files;
                UploadedFileCollection postedFiles = txtFileUpload1.UploadedFiles;
                for (int i = 0; i < postedFiles.Count; i++)
                {
                    UploadedFile postedFile = postedFiles[i];
                    string path = HttpContext.Current.Request.MapPath("~/");
                    string extn = Path.GetExtension(postedFile.FileName);

                    if (extn != ".json")
                    {
                        ucError.ErrorMessage = "Application will accept only .json files.";
                        ucError.Visible = true;
                        return;
                    }

                    if (!Directory.Exists(path + "Attachments/temp/"))
                        Directory.CreateDirectory(path + "Attachments/temp/");

                    postedFile.SaveAs(path + "Attachments/temp/" + dtKey + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')));
                    UplodJson(path + "Attachments/temp/" + dtKey + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')));
                }
            }
            catch(Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

    }

    private void UplodJson(string filepath)
    {
        string jsonString = File.ReadAllText(filepath);
        string json = "";
        using (StringReader sr = new StringReader(jsonString))
        using (JsonTextReader jr = new JsonTextReader(sr))
        {
            json = ChangeNumericalPropertyNames(jr);
        }
        JavaScriptSerializer js = new JavaScriptSerializer();
        RootObject root = new RootObject();
        root  = js.Deserialize<RootObject>(json);


        PhoenixVesselPositionEUMRVMPUploadData.UploadMPA1(int.Parse(ucVessel.SelectedVessel));
        //B1 TABLE
        N1 n1 = new N1();
        n1 = root.n1;
        if (n1 != null)
        {
            PhoenixVesselPositionEUMRVMPUploadData.UploadMPB1(int.Parse(ucVessel.SelectedVessel), n1.l, n1.m);
        }

        //B2 TABLE
        N2 n2 = new N2();
        n2 = root.n2;
        if (n2 != null)
        {
            PhoenixVesselPositionEUMRVMPUploadData.UploadB2(n2.g, n2.h, n2.i);
        }

        //B3 TABLE
        List<N3> in3 = new List<N3>();
        in3 = root.n3;

        if(in3.Count>0)
        {
            PhoenixVesselPositionEUMRVMPUploadData.DeleteMPB3(int.Parse(ucVessel.SelectedVessel));

            string[] stringSeparators = new string[] { "\r\n" };
            foreach (N3 n3 in in3)
            {
                string[] temp = n3.n3.Split(stringSeparators,StringSplitOptions.None);
                int? power = null;
                int? year = null;
                decimal? sfoc = null;

                if (temp.Length > 0)
                {
                    for(int i = 0; i < temp.Length; i++)
                    {
                        if (i == 0)
                        {
                            power = General.GetNullableInteger(Regex.Match(temp[0], @"\d+").Value);
                        }
                        else if (i == 1)
                        {
                            year = General.GetNullableInteger(Regex.Match(temp[1], @"\d+").Value);
                        }else if (i == 2)
                        {
                            sfoc = General.GetNullableDecimal(Regex.Replace(temp[2], @"[^-?\d+\.]", ""));
                        }
                    }
                }
                StringBuilder fueltype = new StringBuilder();
                foreach(string n4 in n3.n4)
                {
                    fueltype.Append(n4);
                    fueltype.Append(",");
                }

                PhoenixVesselPositionEUMRVMPUploadData.UploadMPB3Insert(int.Parse(ucVessel.SelectedVessel), int.Parse(n3.n1), n3.n2, power, year, sfoc, fueltype.ToString());

            }

            PhoenixVesselPositionEUMRVMPUploadData.UploadMPB3(int.Parse(ucVessel.SelectedVessel));
        }

        //B5 TABLE
        N5 B5 = new N5();
        B5 = root.n5;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("B.5", B5.a, null, B5.c, B5.d, B5.e, B5.f, null, null, null, null, null, null, null);

        //C.1 TABLE
        N6 c_1 = new N6();
        c_1 = root.n6;
        PhoenixVesselPositionEUMRVMPUploadData.UploadMPC1(int.Parse(ucVessel.SelectedVessel), c_1.a, c_1.b, c_1.c, c_1.d);

        //C.2.1 TABLE
        List<N7> in7 = new List<N7>();
        in7 = root.n7;
        if (in7.Count > 0)
        {
            PhoenixVesselPositionEUMRVMPUploadData.DeleteC21(int.Parse(ucVessel.SelectedVessel));
            foreach (N7 n7 in in7)
            {
                PhoenixVesselPositionEUMRVMPUploadData.UploadC21(int.Parse(ucVessel.SelectedVessel), n7.n1, n7.n2);
            }
        }

        //C.2.2 TABLE
        N8 c_2_2 = new N8();
        c_2_2 = root.n8;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.2.2", c_2_2.a, null, c_2_2.c, c_2_2.d, c_2_2.e, c_2_2.f, null, null, null, null, null, null, null);

        //C.2.3 TABLE
        N9 c_2_3 = new N9();
        c_2_3 = root.n9;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.2.3", c_2_3.a, null, c_2_3.c, c_2_3.d, null, null, null, null, null, null, null, null, null);


        //C.2.4 TABLE
        List<N10> in10 = new List<N10>();
        in10 = root.n10;
        if (in10.Count > 0)
        {
            PhoenixVesselPositionEUMRVMPUploadData.DeleteC24(int.Parse(ucVessel.SelectedVessel));
            foreach (N10 n10 in in10)
            {
                PhoenixVesselPositionEUMRVMPUploadData.UploadMPC24(int.Parse(ucVessel.SelectedVessel), n10.n1, n10.n2, n10.n3);
            }
        }

        //C.2.5 TABLE
        N11 c_2_5 = new N11();
        c_2_5 = root.n11;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.2.5", c_2_5.a, null, c_2_5.c, c_2_5.d, c_2_5.e, c_2_5.f, null, null, null, null, null, null, null);

        //C.2.6 TABLE
        List<N12> in12 = new List<N12>();
        in12 = root.n12;
        if (in12.Count > 0)
        {
            PhoenixVesselPositionEUMRVMPUploadData.DeleteC26(int.Parse(ucVessel.SelectedVessel));
            foreach (N12 n12 in in12)
            {
                PhoenixVesselPositionEUMRVMPUploadData.UploadMPC26(int.Parse(ucVessel.SelectedVessel), n12.n1, n12.n2, n12.n3);
            }
        }

        //C.2.7 TABLE
        List<N13> in13 = new List<N13>();
        in13 = root.n13;
        if (in13.Count > 0)
        {
            PhoenixVesselPositionEUMRVMPUploadData.DeleteC27(int.Parse(ucVessel.SelectedVessel));
            foreach (N13 n13 in in13)
            {
                decimal v = decimal.Parse("0.00");
                v = (General.GetNullableDecimal(n13.n3.Substring(0, n13.n3.Length - 1)) != null) ? decimal.Parse(n13.n3.Substring(0, n13.n3.Length - 1)) : v;
                PhoenixVesselPositionEUMRVMPUploadData.UploadMPC27(int.Parse(ucVessel.SelectedVessel), n13.n1, n13.n2, v);
            }
        }
        //C.2.8 TABLE
        N14 c_2_8 = new N14();
        c_2_8 = root.n14;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.2.8", c_2_8.a, null, c_2_8.c, c_2_8.d, c_2_8.e, c_2_8.f, null, null, null, null, null, null, null);

        //C.2.10 TABLE
        N16 c_2_10 = new N16();
        c_2_10 = root.n16;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.2.10", c_2_10.a, null, c_2_10.c, c_2_10.d, c_2_10.f, c_2_10.g, null, c_2_10.e, null, null, null, null, null);

        //C.2.11 TABLE
        N17 c_2_11 = new N17();
        c_2_11 = root.n17;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.2.11", c_2_11.a, null, c_2_11.c, c_2_11.d, c_2_11.f, c_2_11.g, null, c_2_11.e, null, null, null, null, null);

        //C.2.12 TABLE
        N18 c_2_12 = new N18();
        c_2_12 = root.n18;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.2.12", c_2_12.a, null, c_2_12.c, c_2_12.d, c_2_12.f, c_2_12.g, null, c_2_12.e, null, null, null, null, null);

        N19 c_3 = new N19();
        c_3 = root.n19;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.3", c_3.a, null, c_3.c, c_3.d, c_3.f, c_3.g, null, null, c_3.e, null, null, null, null);

        N20 c_4_1 = new N20();
        c_4_1 = root.n20;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.4.1", c_4_1.a, null, c_4_1.c, c_4_1.d, c_4_1.f, c_4_1.g, null, null, c_4_1.e, null, null, null, null);

        N21 c_4_2 = new N21();
        c_4_2 = root.n21;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.4.2", c_4_2.a, null, c_4_2.c, c_4_2.d, c_4_2.f, c_4_2.g, null, null, c_4_2.e, null, null, null, null);

        N22 c_5_1 = new N22();
        c_5_1 = root.n22;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.5.1", c_5_1.a, null, c_5_1.c, c_5_1.e, c_5_1.g, c_5_1.h, null, c_5_1.f, null, null, null, null, c_5_1.d);

        N23 c_5_2 = new N23();
        c_5_2 = root.n23;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.5.2", c_5_2.a, null, c_5_2.c, c_5_2.d, c_5_2.f, c_5_2.g, null, c_5_2.e, null, null, null, null, null);

        N24 c_6_1 = new N24();
        c_6_1 = root.n24;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.6.1", c_6_1.a, null, c_6_1.c, c_6_1.d, c_6_1.f, c_6_1.g, null, c_6_1.e, null, null, null, null, null);

        N25 c_6_2 = new N25();
        c_6_2 = root.n25;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("C.6.2", c_6_2.a, null, c_6_2.c, c_6_2.d, c_6_2.f, c_6_2.g, null, c_6_2.e, null, null, null, null, null);

        N26 d_1 = new N26();
        d_1 = root.n26;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("D.1", null, null, null, d_1.d, d_1.f, d_1.g, null, d_1.b, d_1.e, d_1.a, d_1.c, null, null);

        N27 d_2 = new N27();
        d_2 = root.n27;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("D.2", null, null, null, d_2.c, d_2.e, d_2.f, null, d_2.a, d_2.d, null, d_2.b, null, null);

        N28 d_3 = new N28();
        d_3 = root.n28;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("D.3", null, null, null, d_3.c, d_3.e, d_3.f, null, d_3.a, d_3.d, null, d_3.b, null, null);

        N29 d_4 = new N29();
        d_4 = root.n29;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("D.4", null, null, null, d_4.c, d_4.e, d_4.f, null, d_4.a, d_4.d, null, d_4.b, null, null);


        N30 E_1 = new N30();
        E_1 = root.n30;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("E.1", E_1.a, null, E_1.c, E_1.d, E_1.e, E_1.f, null, null, null, null, null, null, null);

        N31 E_2 = new N31();
        E_2 = root.n31;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("E.2", E_2.a, null, E_2.b, E_2.c, E_2.d, E_2.e, null, null, null, null, null, E_2.f, null);

        N32 E_3 = new N32();
        E_3 = root.n32;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("E.3", E_3.a, null, E_3.c, E_3.d, E_3.e, E_3.f, null, null, null, null, null, null, null);

        N33 E_4 = new N33();
        E_4 = root.n33;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("E.4", E_4.a, null, E_4.c, E_4.d, E_4.e, E_4.f, null, null, null, null, null, null, null);

        N34 E_5 = new N34();
        E_5 = root.n34;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("E.5", E_5.a, null, E_5.c, E_5.d, E_5.e, E_5.f, null, null, null, null, null, null, null);

        N35 E_6 = new N35();
        E_6 = root.n35;
        PhoenixVesselPositionEUMRVMPUploadData.UploadEUMRVProcedureDetail("E.6", E_6.a, null, E_6.c, E_6.d, E_6.e, E_6.f, null, null, null, null, null, null, null);

        //upload F1
        List<N36> if_1 = new List<N36>();
        if_1 = root.n36;
        if (if_1.Count > 0)
        {
            foreach (N36 f1 in if_1)
            {
                PhoenixVesselPositionEUMRVMPUploadData.UploadMPF1(f1.n1, f1.n2);
            }
        }


    }
    public static string ChangeNumericalPropertyNames(JsonReader reader)
    {
        JObject jo = JObject.Load(reader);
        ChangeNumericalPropertyNames(jo);
        return jo.ToString();
    }
    public static void ChangeNumericalPropertyNames(JObject jo)
    {
        foreach (JProperty jp in jo.Properties().ToList())
        {
            if (jp.Value.Type == JTokenType.Object)
            {
                ChangeNumericalPropertyNames((JObject)jp.Value);
            }
            else if (jp.Value.Type == JTokenType.Array)
            {
                foreach (JToken child in jp.Value)
                {
                    if (child.Type == JTokenType.Object)
                    {
                        ChangeNumericalPropertyNames((JObject)child);
                    }
                }
            }

            if (Regex.IsMatch(jp.Name, @"^\d"))
            {
                string name = "n" + jp.Name;
                jp.Replace(new JProperty(name, jp.Value));
            }
        }
    }
}
