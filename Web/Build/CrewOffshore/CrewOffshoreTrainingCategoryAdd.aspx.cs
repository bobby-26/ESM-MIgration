using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreTrainingCategoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuTrainingCategory.AccessRights = this.ViewState;
            MenuTrainingCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["CategoryID"] != null)
                {
                    ViewState["CategoryID"] = Request.QueryString["CategoryID"].ToString();
                  //  EditTrainingCategory();
                }
                BindAnswers();
                EditTrainingCategory();
            }

        }
    }
    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //    if (Request.QueryString["CategoryID"] != null)
    //    {
    //        ViewState["CategoryID"] = Request.QueryString["CategoryID"].ToString();
    //        EditTrainingCategory(int.Parse(Request.QueryString["CategoryID"].ToString()));
    //    }
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["CategoryID"] = null;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindAnswers()
    {

        chkRankGroup.DataSource = PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO");
        chkRankGroup.DataBindings.DataTextField = "FLDHARDNAME";
        chkRankGroup.DataBindings.DataValueField = "FLDHARDCODE";
        chkRankGroup.DataBind();

    }

    private void EditTrainingCategory()
    {
        // if (General.GetNullableInteger(ViewState["CategoryID"].ToString()) != null)
        if (Request.QueryString["CategoryID"] == null)
        {
            BindAnswers();
        }
        else
        {
            DataSet ds = PhoenixCrewOffshoreTrainingCategory.EditTrainingCategory(int.Parse(ViewState["CategoryID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCategory.Text = dr["FLDCATEGORYNAME"].ToString();
                General.RadBindCheckBoxList(chkRankGroup, dr["FLDRANKGROUPLIST"].ToString());
            }
        }
    }

    private bool IsValidData(string Category, string rank)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Category.Equals(""))
            ucError.ErrorMessage = "Category is required.";

        if (rank == null || rank == "")
            ucError.ErrorMessage = "Rank Category is required";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["CategoryID"] = null;
        txtCategory.Text = "";

    }
    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)

        {
            if (item.Selected == true)
            {
                // list = list + item.Value.ToString() + ",";
                list += item.Value + ",";
            }
        }
        if (list != "")
        {
            list = "," + list;
        }

        return list;
    }
    protected void MenuTrainingCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='Training Category'>" + "\n";
            scriptClosePopup += "fnReloadList('TrainingCategory');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='TrainingCategoryNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["CategoryID"] != null)
                {
                    string source = ReadCheckBoxList(chkRankGroup);
                    if (!IsValidData(txtCategory.Text, source))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixCrewOffshoreTrainingCategory.UpdateTrainingCategory(int.Parse(ViewState["CategoryID"].ToString())
                         , txtCategory.Text
                         , null
                         , source);


                    ucStatus.Text = "Information Updated successfully.";
                }
                else
                {
                    string source = ReadCheckBoxList(chkRankGroup);
                    if (!IsValidData(txtCategory.Text, source))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixCrewOffshoreTrainingCategory.InsertTrainingCategory(txtCategory.Text
                        , null, source);


                    ucStatus.Text = "Information Added";

                }
                // Page.ClientScript.RegisterStartupScript(typeof(Page), "TrainingCategory", scriptClosePopup);
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

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

}