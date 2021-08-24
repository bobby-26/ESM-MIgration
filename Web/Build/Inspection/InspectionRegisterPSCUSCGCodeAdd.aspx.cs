using System;
using System.Web.UI;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegisterPSCUSCGCodeAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuPSCUSCGCodeAdd.AccessRights = this.ViewState;
            MenuPSCUSCGCodeAdd.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PSCUSCGID"] = "";

                if (Request.QueryString["PSCUSCGID"] != null && Request.QueryString["PSCUSCGID"].ToString() != string.Empty)
                    ViewState["PSCUSCGID"] = Request.QueryString["PSCUSCGID"].ToString();

                BindChapter();
                BindParent();
                BindPSCUSCGCode();

            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    public void BindPSCUSCGCode()
    {
        try
        {
            if (ViewState["PSCUSCGID"] != null && ViewState["PSCUSCGID"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixInspectionRegisterPSCUSCG.EditPSCUSCGCode(new Guid(ViewState["PSCUSCGID"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtSFICode.Text = ds.Tables[0].Rows[0]["FLDSFICODE"].ToString();
                   txtDescription.Text = ds.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
                    txtPSCMOU.Text = ds.Tables[0].Rows[0]["FLDPSCMOU"].ToString();
                    txtUSCGMOU.Text = ds.Tables[0].Rows[0]["FLDUSCGMOU"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
                    ddlChapter.SelectedValue = ds.Tables[0].Rows[0]["FLDCHAPTERID"].ToString();
                    ddlParent.SelectedValue = ds.Tables[0].Rows[0]["FLDPARENTID"].ToString();
                    cbActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindChapter()
    {
        ddlChapter.DataSource = PhoenixInspectionRegisterPSCUSCG.ListInspectionPSCUSCGChapter();
        ddlChapter.DataTextField = "FLDCHAPTERNAME";
        ddlChapter.DataValueField = "FLDCHAPTERID";
        ddlChapter.DataBind();

    }
    private void BindParent()
    {
        ddlParent.DataSource = PhoenixInspectionRegisterPSCUSCG.ListInspectionPSCUSCGParent();
        ddlParent.DataTextField = "FLDDESCRIPTION";
        ddlParent.DataValueField = "FLDINSPECTIONPSCUSCGID";
        ddlParent.DataBind();

    }
    protected void MenuPSCUSCGCodeAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                InserUpdatetPSCUSCG();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void InserUpdatetPSCUSCG()
    {
        try
        {
            if (!IsValidPSCUSCG())
            {
                ucError.Visible = true;
                return;
            }

            int Activeyn = cbActiveyn.Checked == true ? 1 : 0;
            Guid NewPSCUSCGid = new Guid();

            PhoenixInspectionRegisterPSCUSCG.InsertPSCUSCGCode(General.GetNullableGuid(ViewState["PSCUSCGID"].ToString())
                , General.GetNullableString(txtSFICode.Text.Trim())
                , General.GetNullableString(txtDescription.Text.Trim())
                , General.GetNullableString(txtPSCMOU.Text.Trim())
                , General.GetNullableString(txtUSCGMOU.Text.Trim())
                , General.GetNullableString(txtRemarks.Text.Trim())
                , General.GetNullableGuid(ddlChapter.SelectedValue)
                , General.GetNullableGuid(ddlParent.SelectedValue)
                , Activeyn
                , ref NewPSCUSCGid);

            ViewState["PSCUSCGID"] = NewPSCUSCGid;
            ucStatus.Text = "Information updated.";

            String script = String.Format("javascript:fnReloadList('PSCUSCG',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    private bool IsValidPSCUSCG()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtDescription.Text.Trim()) == null)
            ucError.ErrorMessage = "Description is required.";

        if (General.GetNullableGuid(ddlChapter.SelectedValue) == null)
            ucError.ErrorMessage = "Chapter is required.";

        return (!ucError.IsError);
    }

}