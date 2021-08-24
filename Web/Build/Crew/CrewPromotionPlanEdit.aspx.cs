using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI;


public partial class CrewPromotionPlanEdit : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarcopy = new PhoenixToolbar();
            toolbarcopy.AddButton("Save", "SAVE", ToolBarDirection.Right);
            menuplan.AccessRights = this.ViewState;
            menuplan.MenuList = toolbarcopy.Show();

            if (!IsPostBack)
            {
                ViewState["ID"] = "";
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
                {
                    ViewState["ID"] = Request.QueryString["id"].ToString();
                   
                    BindData();
                }
                else
                {
                    BindFields();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void menuplan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string isactive = "0";

            if (chkActive.Checked == true)
                isactive = "1";


            string Script = "";
            Script += "<script language='JavaScript' id='BookMarkScript'>";
            Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>";

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
                {

                    PhoenixCrewPromotionSummary.CrewPromotionPlanUpdate(new Guid(ViewState["ID"].ToString()), General.GetNullableInteger(isactive), txtRemarks.Text);
                    ucStatus.Text = "Saved Successfully";

                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);

                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                }
                else
                {
                    Guid? id = null;

                    PhoenixCrewPromotionSummary.CrewPromotionPlanInsert(General.GetNullableInteger(Filter.CurrentCrewSelection)
                                    , General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString())
                                    , txtRemarks.Text, ref id);

                    ViewState["ID"] = id;
                    chkActive.Enabled = true;
                    chkActive.Checked = true;


                    ucStatus.Text = "Saved Successfully";

                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }


    protected void BindFields()
    {

        DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

        if (dt.Rows.Count > 0)
        {
            chkActive.Checked = true;
            ucRankFrom.SelectedRank = "";
            ucRankFrom.SelectedRank = dt.Rows[0]["FLDRANK"].ToString();
            ViewState["FLDRANKFROM"] = dt.Rows[0]["FLDRANK"].ToString();

        }
    }
    protected void BindData()
    {

        DataTable dt = PhoenixCrewPromotionSummary.CrewPromotionPlanEdit(new Guid(ViewState["ID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucRankFrom.SelectedRank = "";
            ucRankFrom.SelectedRank = dr["FLDCURRENTRANK"].ToString();
            ViewState["FLDRANKFROM"] = dr["FLDCURRENTRANK"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            if (dt.Rows[0]["FLDISACTIVE"].ToString() == "1")
            {
                chkActive.Enabled = true;
                chkActive.Checked = true;
        
            }

        }

    }


}