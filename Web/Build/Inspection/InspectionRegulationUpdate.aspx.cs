using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationUpdate : PhoenixBasePage
{
    Guid? MocId = null;
    Guid? RegulationId = null;
    protected void Page_Load(object sender, EventArgs e)
    {
     
        if (Request.QueryString["RegulationId"] != null)
        {
            RegulationId = new Guid(Request.QueryString["RegulationId"]);
        }
        if (Request.QueryString["Mocid"] != null)
        {
            MocId = new Guid(Request.QueryString["Mocid"]);
        }
        ShowToolBar();
        if (IsPostBack == false)
        {
            GetRegulationDetail();
        }
        
    }
    private void bindvesseltype(string VesselTypeList)
    {
        DataSet ds = PhoenixRegistersVesselType.ListVesselType(0);
        chkvesselList.DataSource = ds;
        chkvesselList.DataBind();

        foreach (RadListBoxItem li in chkvesselList.Items)
        {
            if (VesselTypeList != null)
            {
                string[] slist = VesselTypeList.Split(',');
                foreach (string s in slist)
                {
                    if (li.Value.Equals(s))
                    {
                        li.Checked = true;
                    }
                }
            }
        }
    }
    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        if (MocId != null)
        {
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        }
        //toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbarmain.AddButton("Update", "UPDATE", ToolBarDirection.Right);
        NewRegulation.AccessRights = this.ViewState;
        NewRegulation.MenuList = toolbarmain.Show();
    }
    protected void NewRegulation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                string IssuedDate = txtIssuedDate.Text;
                string IssuedBy = txtIssuedBy.Text;
                string Title = txtTitle.Text;
                string Description = txtDescription.Text;
                string ActionRequired = txtActionRequired.Text;
                string DueDate = txtDuedate.Text;
                string Remarks = txtRemarks.Text;
                int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                // string VesselType = chkvesselList.SelectedVesseltype;
                string VesselType = "";
                foreach (RadListBoxItem li in chkvesselList.Items)
                {
                    if (li.Checked)
                    {
                        if (VesselType == "")
                            VesselType = ",";
                        VesselType += li.Value + ",";
                    }
                }
                VesselType = General.GetNullableString(VesselType);
                if (RegulationId.HasValue == false)
                {
                    throw new ArgumentException("Regulation is required");
                }
                if (IsValidReport(IssuedDate, IssuedBy, Title, Description, ActionRequired, DueDate, VesselType))
                {
                    PhoenixInspectionNewRegulation.RegulationUpdate(RegulationId, IssuedDate, IssuedBy, Title, Description, ActionRequired, usercode, Remarks, VesselType, DueDate);
                    ucStatus.Text = "Saved Successfully";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', 'MoreInfo', null);", true);
                }
                else
                {
                    ucError.Visible = true;
                }
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearUserInput();
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?MOCID=" + MocId);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidReport(string IssuedDate, string IssuedBy, string Title, string Description, string ActionRequired, string DueDate, string VesselType)
    {
        bool validatePass = true;
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrWhiteSpace(IssuedDate))
        {
            ucError.ErrorMessage = "Issued Date Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(IssuedBy))
        {
            ucError.ErrorMessage = "Issued By Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            ucError.ErrorMessage = "Title Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            ucError.ErrorMessage = "Description Cannot be Empty";
            validatePass = false;
        }

        if (string.IsNullOrWhiteSpace(ActionRequired))
        {
            ucError.ErrorMessage = "Action Required Cannot be Empty";
            validatePass = false;
        }

        if (String.IsNullOrWhiteSpace(VesselType))
        {
            ucError.ErrorMessage = "Please select a vessel type";
            validatePass = false;
        }

        if (DueDate == null)
        {
            ucError.ErrorMessage = "Please select a Due Date";
            validatePass = false;
        }

        return validatePass;
    }

    private void ClearUserInput()
    {
        txtTitle.Text = String.Empty;
        txtIssuedDate.Text = null;
        txtIssuedBy.Text = null;
        txtDescription.Text = null;
        txtActionRequired.Text = null;
        txtRemarks.Text = null;
        txtDuedate.Text = null;
    }

    private void GetRegulationDetail()
    {
        if (RegulationId == null && MocId == null) return;

        DataSet ds = PhoenixInspectionNewRegulation.RegulationEdit(RegulationId, MocId);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            txtIssuedDate.Text = row["FLDISSUEDATE"].ToString();
            txtIssuedBy.Text = row["FLDISSUEDBYNAME"].ToString();
            txtTitle.Text = row["FLDTITLE"].ToString();
            txtDescription.Text = row["FLDDESCRIPTION"].ToString();
            txtActionRequired.Text = row["FLDACTIONREQUIRED"].ToString();
            //chkvesselList.SelectedVesseltype = row["FLDVESSELTYPE"].ToString();
            txtRemarks.Text = row["FLDREMARKS"].ToString();
            if (row["FLDDUEDATE"] != null) txtDuedate.Text = row["FLDDUEDATE"].ToString();
            if (RegulationId.HasValue == false) RegulationId = new Guid(row["FLDREGULATIONID"].ToString());
            bindvesseltype(row["FLDVESSELTYPE"].ToString());
        }
    }
}