using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI;

public partial class RegisterCrewPromotionCopy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbarcopy = new PhoenixToolbar();
            toolbarcopy.AddButton("Copy", "COPY",ToolBarDirection.Right);
            menucopy.AccessRights = this.ViewState;
            menucopy.MenuList = toolbarcopy.Show();

            if (!IsPostBack)
            {
                ViewState["ID"] = "";
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
                {
                    ViewState["ID"] = Request.QueryString["id"].ToString();
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

    protected void menucopy_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("COPY"))
            {
                if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
                {
                    PhoenixRegisterCrewPromotionConfiguration.CrewPromotionConfigurationRankToCopy(new Guid(ViewState["ID"].ToString()),General.GetNullableInteger(ucRankTo.SelectedValue));
                    ucStatus.Text = "Copied Successfully";
                    
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }



    protected void ChkRankProm()
    {
        ucRankTo.Items.Clear();
        ucRankTo.SelectedValue = "";
        ucRankTo.Text = "";

        ucRankTo.DataSource = PhoenixRegistersRank.ListRankFilter(null, null, General.GetNullableInteger(ViewState["FLDRANKFROM"].ToString()), 0);
        ucRankTo.DataTextField = "FLDRANKNAME";
        ucRankTo.DataValueField = "FLDRANKID";
        ucRankTo.DataBind();
        ucRankTo.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    protected void BindFields()
    {
        DataTable dt = PhoenixRegisterCrewPromotionConfiguration.CrewPromotionConfigurationEdit(new Guid(ViewState["ID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucRankFrom.SelectedRank = "";
            ucRankFrom.SelectedRank = dr["FLDRANKFROM"].ToString();
            ViewState["FLDRANKFROM"] = dr["FLDRANKFROM"].ToString();
            ChkRankProm();

        }
    }


}