using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersContractingCompanyAdd : PhoenixBasePage
{
    string vesselid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuAppraisal.AccessRights = this.ViewState;
            MenuAppraisal.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = Request.QueryString["companyid"] == null ? "" : Request.QueryString["companyid"].ToString();

                rdbPlaceYn.SelectedValue = "0";
                rbvesselDetailsRequired.SelectedValue = "0";
                rbAddendumRequired.SelectedValue = "0";
                if (ViewState["COMPANYID"].ToString() != "")
                    Edit();
                RemoveEditorToolBarIcons(); // remove the unwanted icons in editor
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAppraisal_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            string MLCCompany = txtMLCcompany.Text + "<br />" + txtMLCcompany1.Text + "<br />" + txtMLCcompany2.Text.Trim();
            if (ViewState["COMPANYID"].ToString() != "")
            {
                PhoenixRegistersContractCompany.UpdateContractCompany(int.Parse(ViewState["COMPANYID"].ToString()), txtShortCode.Text
                                                                             , txtCompanyName.Text
                                                                             , txtContractingParty.Text
                                                                             , null
                                                                             , txtheadercompany.Text
                                                                             , txtHeaderAddress.Text
                                                                             , txtContractingPartyAddress.Text
                                                                             , txtAgentDescription.Text
                                                                             , txtCompanySignature.Content
                                                                             , int.Parse(rdbPlaceYn.SelectedValue)
                                                                             , txtSeafarerSignature.Text
                                                                             , txtSeafarerSignature2.Text
                                                                             , MLCCompany
                                                                             , edtBody.Content
                                                                             , txtversion.Text 
                                                                             , int.Parse(rbvesselDetailsRequired.SelectedValue)
                                                                             ,edtAddendum.Content
                                                                             ,int.Parse(rbAddendumRequired.SelectedValue)
                                                                             );

            }
            else
            {
                if (!IsValidCompany(txtShortCode.Text, txtCompanyName.Text, txtContractingParty.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContractCompany.InsertContractCompany(txtShortCode.Text
                                                                        , txtCompanyName.Text
                                                                        , txtContractingParty.Text
                                                                        , null
                                                                        , txtheadercompany.Text
                                                                        , txtHeaderAddress.Text
                                                                        , txtContractingPartyAddress.Text
                                                                        , txtAgentDescription.Text
                                                                        , txtCompanySignature.Content
                                                                       , int.Parse(rdbPlaceYn.SelectedValue)
                                                                        , txtSeafarerSignature.Text
                                                                        , txtSeafarerSignature2.Text
                                                                        , MLCCompany
                                                                        , edtBody.Content
                                                                        , txtversion.Text
                                                                        , int.Parse(rbvesselDetailsRequired.SelectedValue)
                                                                        , edtAddendum.Content
                                                                        , int.Parse(rbAddendumRequired.SelectedValue));

            }
        }
        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Registers/RegistersContractingCompany.aspx", false);
        }
    }
    private bool IsValidCompany(string shortcode, string companyname, string desc1)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (companyname.Trim().Equals(""))
            ucError.ErrorMessage = "Contracting Party name is required.";
        return (!ucError.IsError);
    }
    private void Edit()
    {
        try
        {
            DataSet ds = PhoenixRegistersContractCompany.EditContractingCompany(int.Parse(ViewState["COMPANYID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtShortCode.Text = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
                txtCompanyName.Text = ds.Tables[0].Rows[0]["FLDCOMPANYNAME"].ToString();
                txtheadercompany.Text = ds.Tables[0].Rows[0]["FLDHEARDER"].ToString();
                txtHeaderAddress.Text = ds.Tables[0].Rows[0]["FLDHEADERADDRESS"].ToString();
                txtAgentDescription.Text = ds.Tables[0].Rows[0]["FLDAGENTDESC"].ToString();
                txtContractingParty.Text = ds.Tables[0].Rows[0]["FLDDESCRIPTION1"].ToString();
                txtContractingPartyAddress.Text = ds.Tables[0].Rows[0]["FLDCONTRACTINGPARTYADDRESS"].ToString();
                txtSeafarerSignature.Text = ds.Tables[0].Rows[0]["FLDSEAFARERSIGN"].ToString();
                txtSeafarerSignature2.Text = ds.Tables[0].Rows[0]["FLDSEAFARERSIGN2"].ToString();
                rdbPlaceYn.SelectedValue = ds.Tables[0].Rows[0]["FLDPLACEYN"].ToString();
                rbvesselDetailsRequired.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELDETAILS"].ToString();
                rbAddendumRequired.SelectedValue = ds.Tables[0].Rows[0]["FLDADDENDUMREQUIREDYN"].ToString();
                string[] mlccompany = ds.Tables[0].Rows[0]["FLDMLCOWNER"].ToString().Split('|');
                if (mlccompany.Length >= 1)
                    txtMLCcompany.Text = mlccompany[0].ToString();
                if (mlccompany.Length >= 2)
                    txtMLCcompany1.Text = mlccompany[1].ToString();
                if (mlccompany.Length >= 3)
                    txtMLCcompany2.Text = mlccompany[2].ToString();
                txtCompanySignature.Content = ds.Tables[0].Rows[0]["FLDCOMPANYSIGN"].ToString();
                txtversion.Text = ds.Tables[0].Rows[0]["FLDVERSION"].ToString();
                edtBody.Content = ds.Tables[0].Rows[0]["FLDTERMSCONDITION"].ToString();
                edtAddendum.Content = ds.Tables[0].Rows[0]["FLDADDENDUM"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {

    }

    protected void RemoveEditorToolBarIcons()
    {
        txtCompanySignature.EnsureToolsFileLoaded();
        edtBody.EnsureToolsFileLoaded();
        edtAddendum.EnsureToolsFileLoaded();
        RemoveButton("ImageManager");
        RemoveButton("DocumentManager");
        RemoveButton("FlashManager");
        RemoveButton("MediaManager");
        RemoveButton("TemplateManager");
        RemoveButton("XhtmlValidator");
        RemoveButton("InsertSnippet");
        RemoveButton("ModuleManager");
        RemoveButton("ImageMapDialog");
        RemoveButton("AboutDialog");
        RemoveButton("InsertFormElement");

        txtCompanySignature.EnsureToolsFileLoaded();
        txtCompanySignature.Modules.Remove("RadEditorHtmlInspector");
        txtCompanySignature.Modules.Remove("RadEditorNodeInspector");
        txtCompanySignature.Modules.Remove("RadEditorDomInspector");
        txtCompanySignature.Modules.Remove("RadEditorStatistics");

        edtBody.EnsureToolsFileLoaded();
        edtBody.Modules.Remove("RadEditorHtmlInspector");
        edtBody.Modules.Remove("RadEditorNodeInspector");
        edtBody.Modules.Remove("RadEditorDomInspector");
        edtBody.Modules.Remove("RadEditorStatistics");

        edtAddendum.EnsureToolsFileLoaded();
        edtAddendum.Modules.Remove("RadEditorHtmlInspector");
        edtAddendum.Modules.Remove("RadEditorNodeInspector");
        edtAddendum.Modules.Remove("RadEditorDomInspector");
        edtAddendum.Modules.Remove("RadEditorStatistics");
    }

    protected void RemoveButton(string name)
    {
        foreach (EditorToolGroup group in txtCompanySignature.Tools)
        {
            EditorTool tool = group.FindTool(name);
            if (tool != null)
            {
                group.Tools.Remove(tool);
            }
        }

    }
}
