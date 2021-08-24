using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Registers_RegisterCrewApprovalSubCategoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("SAVE", "SAVE", ToolBarDirection.Right);
        MenuRemarks.AccessRights = this.ViewState;
        MenuRemarks.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            ViewState["aid"] = null;
            ViewState["lid"] = null;

            if (Request.QueryString["aid"] != null && Request.QueryString["aid"].ToString() != string.Empty)
            {
                ViewState["aid"] = Request.QueryString["aid"].ToString();
                DataTable dt = PhoenixRegistersCrewApprovalCategory.ApprovalCategoryEdit(General.GetNullableGuid(ViewState["aid"].ToString()));
                if (dt.Rows.Count > 0) { lblapprovalcatname.Text = dt.Rows[0]["FLDAPPROVALCATEGORYNAME"].ToString(); }
                chkRankApplicable.DataSource = PhoenixRegistersCrewApprovalCategory.ListRank(General.GetNullableGuid(ViewState["aid"].ToString()), null);
                chkRankApplicable.DataTextField = "FLDRANKNAME";
                chkRankApplicable.DataValueField = "FLDRANKID";
                chkRankApplicable.DataBind();
                chkRankApplicable.Enabled = true;
            }
            if (Request.QueryString["lid"] != null && Request.QueryString["lid"].ToString() != string.Empty)
            {
                ViewState["lid"] = Request.QueryString["lid"].ToString();
                chkRankApplicable.DataSource = PhoenixRegistersCrewApprovalCategory.ListRank(General.GetNullableGuid(ViewState["aid"].ToString()), General.GetNullableGuid(ViewState["lid"].ToString()));
                chkRankApplicable.DataTextField = "FLDRANKNAME";
                chkRankApplicable.DataValueField = "FLDRANKID";
                chkRankApplicable.DataBind();

                DataTable dt = PhoenixRegisterCrewApprovalCategoryLevel.CrewApprovalSubCategoryList(General.GetNullableGuid(ViewState["aid"].ToString()),General.GetNullableGuid(ViewState["lid"].ToString()));

                txtcode.Text = dt.Rows[0]["FLDAPPROVALSUBCATEGORYCODE"].ToString();
                txtcatlevelname.Text = dt.Rows[0]["FLDAPPROVALSUBCATEGORYNAME"].ToString();
                if (chkRankApplicable != null)
                {
                    foreach (ListItem li in chkRankApplicable.Items)
                    {
                        string[] slist = dt.Rows[0]["FLDRANKLIST"].ToString().Split(',');
                        foreach (string s in slist)
                        {
                            if (li.Value.Equals(s))
                            {
                                li.Selected = true;
                            }
                        }
                    }
                }
            }
        }
    }


    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            
            if (General.GetNullableString(txtcode.Text) == null || General.GetNullableString(txtcatlevelname.Text) == null)
            {
                ucError.Text = "Code and Level name required.";
                ucError.Visible = false;
                return;
            }
            string RList = "";
            string RankList = "";
            foreach (ListItem li in chkRankApplicable.Items)
            {
                if (li.Selected)
                {
                    RList += li.Value + ",";
                }
            }

            if (RList != "")
            {
                RankList = "," + RList;
            }
            else
            {
                ucError.ErrorMessage = "RankList Is Required";
            }

            string Script = "";

            if (ViewState["lid"] == null && ViewState["aid"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixRegistersCrewApprovalCategory.CrewApprovalSubCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , new Guid(ViewState["aid"].ToString())
                                                                   , txtcatlevelname.Text
                                                                   , txtcode.Text
                                                                   , RankList
                                                                   );

                 
                }
            }
            else
            {

                PhoenixRegistersCrewApprovalCategory.CrewApprovalSubCategoryUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                   , General.GetNullableGuid(ViewState["lid"].ToString())
                                                                                   , General.GetNullableGuid(ViewState["aid"].ToString())
                                                                                   , txtcatlevelname.Text
                                                                                   , txtcode.Text
                                                                                   , RankList);
            }
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('CI','ifMoreInfo');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = false;
            return;         
        }
    }
}