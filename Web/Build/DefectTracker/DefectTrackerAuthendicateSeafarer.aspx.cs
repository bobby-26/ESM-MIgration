using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using System.Net;
using Telerik.Web.UI;

public partial class DefectTracker_DefectTrackerAuthendicateSeafarer : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMasterName.Attributes.Add("readonly", "readonly");
        lblCheifEngineername.Attributes.Add("readonly", "readonly");
        ucDateofBirth.Attributes.Add("Value", ucDateofBirth.Text);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbaredit = new PhoenixToolbar();
            toolbaredit.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);
            MenuSubmit.AccessRights = this.ViewState;
            MenuSubmit.MenuList = toolbaredit.Show();
        }
    }

    protected void ucVessel_TextChanged(object sender, EventArgs e)
    {
        string vesselid = ucVessel.SelectedVessel;
        //string phoenixurl = ConfigurationManager.AppSettings.Get("phoenixurl").ToString();
        //string xml = "function=GetSeafarerDetails|vesselid=" + vesselid;
        //string url = "http://119.81.76.123/Phoenix/" + "Options/OptionsAuthenticateSeafarer.aspx?function=GetSeafarerDetails&vesselid=" + vesselid;
        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        //byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(xml);
        //req.Method = "POST";
        //req.ContentType = "text/xml;charset=utf-8";
        //req.ContentLength = requestBytes.Length;
        //Stream requestStream = req.GetRequestStream();
        //requestStream.Write(requestBytes, 0, requestBytes.Length);
        //requestStream.Close();

        //HttpWebResponse res = (HttpWebResponse)req.GetResponse();
        //StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
        //string backstr = sr.ReadToEnd();
        //sr.Close();
        //res.Close();


        //string[] names = backstr.Split('|');
        //if (names.Length == 2)
        //{
        //    lblMasterName.Text = names[0];
        //    lblCheifEngineername.Text = names[1];
        //}
    }

    protected void MenuAuthendicate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SUBMIT"))
        {
            if (ucVessel.SelectedVessel == "Dummy" || ucVessel.SelectedVessel == "")
            {
                ucError.HeaderMessage = "Please provide the following required information";
                ucError.ErrorMessage = "Vessel name is required";
                ucError.Visible = true;
                return;
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.UserName.ToUpper() != "SEAFARER")
                Response.Redirect("../DefectTracker/DefectTrackerExportFileDownload.aspx?vesselid=" + ucVessel.SelectedVessel + "&fileno=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&seafarer=" + PhoenixSecurityContext.CurrentSecurityContext.UserName + "&vesselname=" + ucVessel.SelectedVesselName);
            else
            {
                if (!IsValidValues(txtFileNo.Text, ucDateofBirth.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                string vesselid = ucVessel.SelectedVessel;
                string fileno = txtFileNo.Text;
                string dob = ucDateofBirth.Text;
                string phoenixurl = ConfigurationManager.AppSettings.Get("phoenixurl").ToString();
                string xml = "function=ValidateSeafarer|fileno=" + fileno + "|dob=" + dob + "|vesselid=" + vesselid;
                string url = phoenixurl + "Options/OptionsAuthenticateUser.aspx";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(xml);
                req.Method = "POST";
                req.ContentType = "text/xml;charset=utf-8";
                req.ContentLength = requestBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
                string backstr = sr.ReadToEnd();
                res.Close();

                string[] valueList = backstr.Split('|');
                string seafarer = valueList[0];
                string filenumber = valueList[1];

                if (seafarer == "Error")
                {
                    ucError.ErrorMessage = seafarer;
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../DefectTracker/DefectTrackerExportFileDownload.aspx?vesselid=" + ucVessel.SelectedVessel + "&fileno=" + filenumber + "&seafarer=" + seafarer);
            }
        }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private bool IsValidValues(string fileno, string dob)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (fileno == "")
            ucError.ErrorMessage = " Username is required";

        if ((dob == "") || (dob == null))
            ucError.ErrorMessage = "Password is required";

        return (!ucError.IsError);
    }
}
