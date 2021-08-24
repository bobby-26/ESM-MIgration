using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class VesselAccounts_VesselAccountsEmployeeQueryUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCrewList.AccessRights = this.ViewState;
            MenuCrewList.MenuList = toolbar.Show();

            if (Request.QueryString["Signonoffid"] != null)
            {
                ViewState["Signonoffid"] = Request.QueryString["Signonoffid"].ToString();
                VesselSignOnEdit(int.Parse(Request.QueryString["Signonoffid"].ToString()));
            }

        }

    }

    private void VesselSignOnEdit(int signonoffid)
    {
        try
        {
            DataSet ds = PhoenixCommonVesselAccounts.VesselSignOnEdit(signonoffid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtFileno.Text = dr["FLDFILENO"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
                txtRank.Text = dr["FLDSIGNONRANKCODE"].ToString();
                txtSignondate.Text = dr["FLDSIGNONDATE"].ToString();
                txtReliefDue.Text = dr["FLDRELIEFDUEDATE"].ToString();
                chkAllowPayrollYN.Checked = dr["FLDACCOUNTSYN"].ToString() == "1" ? true : false;
             
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='CrewList'>" + "\n";
            scriptClosePopup += "fnReloadList('CrewList');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='CrewListNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {


                if (ViewState["Signonoffid"] != null)
                {
                   
                    PhoenixCommonVesselAccounts.Updateaccountyn(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["Signonoffid"].ToString()),
                                                                General.GetNullableInteger(chkAllowPayrollYN.Checked == true ? "1" : "0"));

                }

                Page.ClientScript.RegisterStartupScript(typeof(Page), "CrewList", scriptClosePopup);
              

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
            ViewState["Signonoffid"] = null;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Reset()
    {
        ViewState["Signonoffid"] = null;
        txtFileno.Text = "";
        txtName.Text = "";
        txtRank.Text = "";
        txtSignondate.Text = "";
        txtReliefDue.Text = "";
    
    }

}
