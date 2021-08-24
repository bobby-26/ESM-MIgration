using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegisterPersonalProfileRankMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuTemplateMapping.AccessRights = this.ViewState;
            MenuTemplateMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["CATEGORYID"] = "";
                ViewState["EVALUATION"] = "";
                ViewState["ID"] = "";

                if (Request.QueryString["QuestionID"] != null && Request.QueryString["QuestionID"].ToString() != string.Empty)
                    ViewState["ID"] = Request.QueryString["QuestionID"].ToString();

                if (Request.QueryString["RankCategory"] != null && Request.QueryString["RankCategory"].ToString() != string.Empty)
                    ViewState["RANKCATEGORY"] = Request.QueryString["RankCategory"].ToString();

                BindSource();
                BindMapping();
                BindProfilequestion();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindProfilequestion()
    {
        if (ViewState["ID"].ToString() != null && ViewState["ID"].ToString() != "")
        {
            DataSet dt = PhoenixRegistersAppraisalProfileQuestion.EditAppraisalProfileQuestion(int.Parse(ViewState["ID"].ToString()));
            if (dt.Tables[0].Rows.Count > 0)
            {
                txtCategory.Text = dt.Tables[0].Rows[0]["FLDPROFILECATEGORY"].ToString();
                txtEval.Text = dt.Tables[0].Rows[0]["FLDPROFILEQUESTION"].ToString();
            }
        }
    }

    protected void BindSource()
    {
        string RankCategory;

        RankCategory = (ViewState["RANKCATEGORY"] == null) ? null : (ViewState["RANKCATEGORY"].ToString());

        DataSet ds = PhoenixRegisterPersonalProfileRankMap.PersonalProfileRankList(RankCategory);

        cblRank.DataSource = ds.Tables[0];
        cblRank.DataBindings.DataTextField = "FLDRANKNAME";
        cblRank.DataBindings.DataValueField = "FLDRANKID";
        cblRank.DataBind();
    }

    protected void BindMapping()
    {
        BindSource();
        if (General.GetNullableInteger(ViewState["ID"].ToString()) != null)
        {
            DataSet ds = PhoenixRegisterPersonalProfileRankMap.GetPersonalProfileRankMappingList(
               General.GetNullableInteger(ViewState["ID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    //if (cblRank.Items[].FindByValue(ds.Tables[0].Rows[i]["FLDRANKID"].ToString()) != null)
                    //    cblRank.Items.FindByValue(ds.Tables[0].Rows[i]["FLDRANKID"].ToString()).Selected = true;
                    cblRank.SelectedValue = ds.Tables[0].Rows[i]["FLDRANKID"].ToString();
                }
            }
        }
    }

    protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strcategoryid = new StringBuilder();

                StringBuilder strRankid = new StringBuilder();
                foreach (ButtonListItem item in cblRank.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strRankid.Append(item.Value.ToString());
                        strRankid.Append(",");
                    }
                }

                if (strRankid.Length > 1)
                {
                    strRankid.Remove(strRankid.Length - 1, 1);

                    PhoenixRegisterPersonalProfileRankMap.InsertPersonalProfileRankMapping(
                               General.GetNullableInteger(Request.QueryString["QuestionID"])
                              , strRankid.ToString());
                }
                else
                {
                    PhoenixRegisterPersonalProfileRankMap.InsertPersonalProfileRankMapping(
                               General.GetNullableInteger(Request.QueryString["QuestionID"])
                              , null);
                }

                ucStatus.Text = "Rank mapped successfully.";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
