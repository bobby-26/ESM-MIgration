using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionSealUsageRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["SEALUSAGEID"] = "";
                ViewState["REMARKSID"] = "";
                ViewState["TYPE"] = "";

                if (Request.QueryString["remarksid"] != null)
                    ViewState["REMARKSID"] = Request.QueryString["remarksid"].ToString();
                if (Request.QueryString["sealusageid"] != null)
                    ViewState["SEALUSAGEID"] = Request.QueryString["sealusageid"].ToString();
                if (Request.QueryString["type"] != null)
                    ViewState["TYPE"] = Request.QueryString["type"].ToString();

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuSealUsage.MenuList = toolbar.Show();
               // MenuSealUsage.SetTrigger(pnlSealUsageRemarks);
             
                divRemarksView.Visible = false;
                divRemarksEdit.Visible = true;                
                if (General.GetNullableGuid(ViewState["SEALUSAGEID"].ToString()) != null)
                {
                    DataTable dt = PhoenixInspectionSealUsage.ListSealUsage(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , null
                        , General.GetNullableGuid(ViewState["SEALUSAGEID"].ToString()));

                    if (dt.Rows.Count > 0)
                    {
                            DataRow dr = dt.Rows[0];
                        
                            ucMultiInspection.SelectedValue = dr["FLDUSAGEINSPECTION"].ToString();
                            ucMultiInspection.Text = dr["FLDINSPECTIONNAME"].ToString();
                            ucMultiIncident.SelectedValue = dr["FLDUSAGEINCIDENT"].ToString();
                            ucMultiIncident.Text = dr["FLDINCIDENTNAME"].ToString();
                            txtRemarks.Text = dr["FLDUSAGEREMARKS"].ToString();
                                            
                    }
                }
                if (General.GetNullableInteger(ViewState["REMARKSID"].ToString()) != null)
                {
                     
                        if (ViewState["REMARKSID"].ToString() == "4")
                        {
                            lblInspection.Visible = true;
                            ucMultiInspection.Visible = true;
                            ucMultiInspection.CssClass = "input_mandatory";

                            lblIncident.Visible = false;
                            ucMultiIncident.SelectedValue = "";
                            ucMultiIncident.Text = "";
                            ucMultiIncident.Visible = false;

                            lblRemarks.Visible = false;
                            txtRemarks.Text = "";
                            txtRemarks.Visible = false;
                        }
                        if (ViewState["REMARKSID"].ToString() == "5")
                        {
                            lblInspection.Visible = false;
                            ucMultiInspection.SelectedValue = "";
                            ucMultiInspection.Text = "";
                            ucMultiInspection.Visible = false;

                            lblIncident.Visible = true;
                            ucMultiIncident.Visible = true;
                            ucMultiIncident.CssClass = "input_mandatory";

                            lblRemarks.Visible = false;
                            txtRemarks.Text = "";
                            txtRemarks.Visible = false;
                        }
                        if (ViewState["REMARKSID"].ToString() == "6")
                        {
                            lblInspection.Visible = false;
                            ucMultiInspection.SelectedValue = "";
                            ucMultiInspection.Text = "";
                            ucMultiInspection.Visible = false;

                            lblIncident.Visible = false;
                            ucMultiIncident.SelectedValue = "";
                            ucMultiIncident.Text = "";
                            ucMultiIncident.Visible = false;

                            lblRemarks.Visible = true;
                            txtRemarks.CssClass = "input_mandatory";
                        }
                                        
                }
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuSealUsage_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["TYPE"].ToString() == "1")
                {
                    NameValueCollection nvc = new NameValueCollection();
                    if (General.GetNullableGuid(ViewState["SEALUSAGEID"].ToString()) != null)
                        nvc.Add("sealusageid", ViewState["SEALUSAGEID"].ToString());
                    nvc.Add("ucMultiInspection", General.GetNullableString(ucMultiInspection.Text) == null ? "" : ucMultiInspection.SelectedValue);
                    nvc.Add("ucMultiIncident", General.GetNullableString(ucMultiIncident.Text) == null ? "" : ucMultiIncident.SelectedValue);
                    nvc.Add("txtRemarks", txtRemarks.Text);
                    Filter.CurrentSealUsageRemarks = nvc;

                }
                if (ViewState["TYPE"].ToString() == "2")
                {
                    if (General.GetNullableGuid(ViewState["SEALUSAGEID"].ToString()) != null)
                    {
                        
                        PhoenixInspectionSealUsage.UpdateSealUsageRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(ViewState["SEALUSAGEID"].ToString())
                            , General.GetNullableGuid(General.GetNullableString(ucMultiInspection.Text) == null ?"" : ucMultiInspection.SelectedValue)
                            , General.GetNullableGuid(General.GetNullableString(ucMultiIncident.Text) == null ? "" : ucMultiIncident.SelectedValue)
                            , General.GetNullableString(txtRemarks.Text)
                            );
                        Session["POPUPSAVE"] = "1";
                        //ucError.Visible = true;
                        //ucError.ErrorMessage = "Information Updated";                        
                        ucStatus.Visible = true;                        
                        ucStatus.Text = "Information Updated";
                    }
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);             
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

}
