using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;

public partial class CrewOffshoreTrainingSubCategoryCourseMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            CourseMapping.AccessRights = this.ViewState;
            CourseMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["subcategoryid"] = "";

                if (Request.QueryString["subcategoryid"] != null && Request.QueryString["subcategoryid"].ToString() != "")
                    ViewState["subcategoryid"] = Request.QueryString["subcategoryid"].ToString();
                string doctype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
                BindCourse(ddlTrainingCourse, doctype);
                doctype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
                BindCBT(ddlCBT, doctype);
                EditSubcategory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCourse(DropDownList ddl, string documenttype)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixCrewOffshoreTrainingSubCategory.ListTrainingCourse(documenttype);
        ddl.DataTextField = "FLDCOURSE";
        ddl.DataValueField = "FLDDOCUMENTID";
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void BindCBT(DropDownList ddl, string documenttype)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixCrewOffshoreTrainingSubCategory.ListTrainingCBT(documenttype);
        ddl.DataTextField = "FLDCOURSE";
        ddl.DataValueField = "FLDDOCUMENTID";
        ddl.Items.Insert(0, new ListItem("--Select--", ""));
        ddl.DataBind();
    }

    protected void EditSubcategory()
    {
        DataTable dt = PhoenixCrewOffshoreTrainingSubCategory.EditTrainingSubCategory(General.GetNullableInteger(ViewState["subcategoryid"].ToString()));

        if (dt.Rows.Count > 0)
        {
            txtCategory.Text = dt.Rows[0]["FLDCATEGORYNAME"].ToString();
            txtSubcategory.Text = dt.Rows[0]["FLDSUBCATEGORYNAME"].ToString();
            ddlTrainingCourse.SelectedValue = dt.Rows[0]["FLDTRAININGCOURSEID"].ToString();
            ddlCBT.SelectedValue = dt.Rows[0]["FLDCBTCOURSEID"].ToString();
        }
    }

    protected void CourseMapping_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewOffshoreTrainingSubCategory.UpdateTrainingSubCategoryCourse(int.Parse(ViewState["subcategoryid"].ToString()),
                    General.GetNullableInteger(ddlTrainingCourse.SelectedValue), General.GetNullableInteger(ddlCBT.SelectedValue));
                ucStatus.Text = "Updated successfully.";
                EditSubcategory();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //                     "BookMarkScript", "fnReloadList('chml', null, true);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
}
