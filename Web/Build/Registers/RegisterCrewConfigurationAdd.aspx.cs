using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegisterCrewConfigurationAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["ID"] = "";

                if (Request.QueryString["Id"] != null)
                {
                    ViewState["ID"] = Request.QueryString["Id"].ToString();
                    EditConfiguration();
                }
                else
                {
                    chkisactive.Checked = true;
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

    private void EditConfiguration()
    {
        try
        {
            if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
            {
                DataTable ds = PhoenixRegisterCrewConfiguration.EditCrewConfiguration(new Guid(ViewState["ID"].ToString()));

                if (ds.Rows.Count > 0)
                {
                    txtCode.Text = ds.Rows[0]["FLDCODE"].ToString();
                    txtDesc.Text = ds.Rows[0]["FLDDESCRIPTION"].ToString();

                    if (ds.Rows[0]["FLDISACTIVE"].ToString() == "1")
                        chkisactive.Checked = true;
                    else
                    {
                        chkisactive.Checked = false;
                    }

                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='crew'>" + "\n";
            scriptClosePopup += "fnReloadList('codehelp1');";
            scriptClosePopup += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
                {
                    PhoenixRegisterCrewConfiguration.UpdateCrewConfig(new Guid(ViewState["ID"].ToString())
                                              ,General.GetNullableString(txtCode.Text.Trim())
                                               , General.GetNullableString(txtDesc.Text.Trim())
                                              , General.GetNullableInteger(chkisactive.Checked == true ? "1" : "0")                                            
                                              );

                    ucStatus.Text = "Information Updated";
                }
                else
                {

                    PhoenixRegisterCrewConfiguration.InsertCrewConfiguration( General.GetNullableString(txtCode.Text.Trim())
                                              , General.GetNullableString(txtDesc.Text.Trim())
                                              , General.GetNullableInteger(chkisactive.Checked == true ? "1" : "0")                                              
                                              );

                    ucStatus.Text = "Information Added";

                }
                Page.ClientScript.RegisterStartupScript(typeof(Page), "crew", scriptClosePopup);

            }
            if (CommandName.ToUpper().Equals("NEW"))
                Reset();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ID"] = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private void Reset()
    {
        ViewState["ID"] = null;
    }


}