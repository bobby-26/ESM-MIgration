using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Telerik.Web.UI;

public partial class DryDockJobGeneralAdd : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["STANDARDJOBID"] = null;
                if (Request.QueryString["STANDARDJOBID"] != null)
                {
                    ViewState["STANDARDJOBID"] = Request.QueryString["STANDARDJOBID"];
                }

                toolbar.AddButton("Details", "DETAIL", ToolBarDirection.Right);
                toolbar.AddButton("List", "LIST",ToolBarDirection.Right);
                
                MenuHeader.AccessRights = this.ViewState;
                MenuHeader.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuStandardJobSpecification.AccessRights = this.ViewState;
                MenuStandardJobSpecification.MenuList = toolbar.Show();
                MenuHeader.SelectedMenuIndex = 0;
                BindCheckBox();

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCheckBox()
    {
        string type = string.Empty;

        cblEnclosed.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(4);
        cblEnclosed.DataTextField = "FLDNAME";
        cblEnclosed.DataValueField = "FLDMULTISPECID";
        cblEnclosed.DataBind();

        cblMaterial.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(1);
        cblMaterial.DataTextField = "FLDNAME";
        cblMaterial.DataValueField = "FLDMULTISPECID";
        cblMaterial.DataBind();

        cblInclude.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(3);
        cblInclude.DataTextField = "FLDNAME";
        cblInclude.DataValueField = "FLDMULTISPECID";
        cblInclude.DataBind();

        cblWorkSurvey.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(2);
        cblWorkSurvey.DataTextField = "FLDNAME";
        cblWorkSurvey.DataValueField = "FLDMULTISPECID";
        cblWorkSurvey.DataBind();

        cblResponsibilty.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(5);
        cblResponsibilty.DataTextField = "FLDNAME";
        cblResponsibilty.DataValueField = "FLDMULTISPECID";
        cblResponsibilty.DataBind();
        if (cblResponsibilty.Items.Count > 0)
            cblResponsibilty.Items[0].Selected = true;

        ddlCostClassification.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(8);
        ddlCostClassification.DataTextField = "FLDNAME";
        ddlCostClassification.DataValueField = "FLDMULTISPECID";
        ddlCostClassification.DataBind();

        ucJobType.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(7);
        ucJobType.DataTextField = "FLDNAME";
        ucJobType.DataValueField = "FLDMULTISPECID";
        ucJobType.DataBind();

       
        radddlbudgetcode.DataSource = PhoenixDryDockOrder.DrydockBudgetcodeList(null);
        radddlbudgetcode.DataTextField = "FLDBUDGETCODE";
        radddlbudgetcode.DataValueField = "FLDBUDGETID";
        radddlbudgetcode.DataBind();
    }
    protected void StandardJobSpecification_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRepairSpec())
                {
                    ucError.Visible = true;
                    return;
                }

                string strMat = ReadCheckBoxList(cblMaterial);
                string strEnc = ReadCheckBoxList(cblEnclosed);
                string strWrk = ReadCheckBoxList(cblWorkSurvey);
                string strInc = ReadCheckBoxList(cblInclude);
                string strRes = ReadCheckBoxList(cblResponsibilty);


                Guid? jobid = null;
                jobid = PhoenixDryDockJobGeneral.InsertDryDockJobGeneral(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    2,//standardjob													
                                                    General.GetNullableString(txtNumber.Text).Trim(),
                                                    txtTitle.Text.Trim(),
                                                    strRes,
                                                    General.GetNullableDecimal(txtDuration.Text),
                                                    null,
                                                    General.GetNullableInteger(ucJobType.SelectedValue),
                                                    General.GetNullableString(txtJobDescription.Text).Trim(),
                                                    null,
                                                    strWrk, strInc, strMat, strEnc, General.GetNullableInteger(ddlCostClassification.SelectedValue),
                                                    ref jobid
                                                    );
                ViewState["STANDARDJOBID"] = jobid;
                //Budget code mapping 
                PhoenixDryDockJobGeneral.DrydockJobGenralBudgetcodeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , jobid
                    , General.GetNullableInteger(radddlbudgetcode.SelectedValue));

                ucStatus.Text = "Details Updated.";
                String script = "javascript:fnJobEdit('" + ViewState["STANDARDJOBID"].ToString() + "'); javascript:fnReloadList('code1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string ReadCheckBoxList(RadListBox cbl)
    {
        string list = string.Empty;

        foreach (RadListBoxItem item in cbl.Items)
        {
            if (item.Checked == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }
    private bool IsValidRepairSpec()
    {

        ucError.HeaderMessage = "Please provide the following required information.";

        if (txtNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Number cannot be blank.";

        if (txtTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Title cannot be blank.";

        if (txtJobDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Job Description cannot be blank.";

        if (General.GetNullableInteger(ucJobType.SelectedValue) == null)
            ucError.ErrorMessage = "Job Type cannot be blank.";

        if (!General.GetNullableInteger(ddlCostClassification.SelectedValue).HasValue)
            ucError.ErrorMessage = "Cost Classification cannot be blank.";

        return (!ucError.IsError);
    }
    protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DETAIL"))
        {
            Response.Redirect("../DryDock/DryDockJobGeneral.aspx?STANDARDJOBID=" + ViewState["STANDARDJOBID"].ToString(), false);
        }
        else if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../DryDock/DryDockJobGeneralList.aspx?STANDARDJOBID=" + ViewState["STANDARDJOBID"].ToString(), false);
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    private bool IsValidRepairJobDetail(string jobdetail, string standardjobid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(standardjobid) == null)
            ucError.ErrorMessage = "Please add Standard job and then add the details";

        if (jobdetail.Trim().Equals(""))
            ucError.ErrorMessage = "Job Detail is required.";

        return (!ucError.IsError);
    }
}
