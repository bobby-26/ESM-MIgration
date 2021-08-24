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

public partial class RegisterCrewApprovalSubCategoryLevel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegisterCrewApprovalSubCategoryLevel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSubCategoryLevel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            gvSubCategoryLevelTab.AccessRights = this.ViewState;
            gvSubCategoryLevelTab.MenuList = toolbar.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindCategory();
                gvSubCategoryLevel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvSubCategoryLevelTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        //int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { };
        string[] alCaptions = { };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //ds = PhoenixRegisterCrewApprovalCategoryLevel(



        //            , sortexpression, sortdirection,
        //            1,
        //            gvSubCategoryLevel.PageSize,
        //            ref iRowCount,
        //            ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvSubCategoryLevel.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvSubCategoryLevel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSubCategoryLevel.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { };
        string[] alCaptions = { };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegisterCrewApprovalCategoryLevel.CrewApproverSubCategoryLevelSearch(General.GetNullableGuid(ddlsubcategory.SelectedValue)
                    , sortexpression, sortdirection,
                    int.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvSubCategoryLevel.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);

        General.SetPrintOptions("gvSubCategoryLevel", "", alCaptions, alColumns, ds);

        gvSubCategoryLevel.DataSource = ds;
        gvSubCategoryLevel.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvSubCategoryLevel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "ADD")
            {
                RadTextBox txtLevelnameAdd = (RadTextBox)e.Item.FindControl("txtLevelnameAdd");
                RadComboBox ddlConfignameadd = (RadComboBox)e.Item.FindControl("ddlConfignameadd");

                PhoenixRegisterCrewApprovalCategoryLevel.CrewSubCategoryLevelInsert(General.GetNullableGuid(ddlsubcategory.SelectedValue)
                                                                                    , txtLevelnameAdd.Text
                                                                                    , General.GetNullableGuid(ddlConfignameadd.SelectedValue));
                gvSubCategoryLevel.Rebind();

            }
            else if (e.CommandName.ToUpper() == "SAVE")
            {
                RadTextBox txtLevelNameEdit = (RadTextBox)e.Item.FindControl("txtLevelNameEdit");
                RadComboBox ddlConfignameedit = (RadComboBox)e.Item.FindControl("ddlConfignameedit");
                RadLabel lblsubCategoryLevelIdEdit = (RadLabel)e.Item.FindControl("lblsubCategoryLevelIdEdit");


                PhoenixRegisterCrewApprovalCategoryLevel.CrewSubCategoryLevelUpdate(General.GetNullableGuid(lblsubCategoryLevelIdEdit.Text)
                                                                                    , txtLevelNameEdit.Text
                                                                                    , General.GetNullableGuid(ddlConfignameedit.SelectedValue));
                gvSubCategoryLevel.Rebind();
            }
            else if(e.CommandName.ToUpper() == "DELETE")
            {
                RadLabel lblsubCategoryLevelId = (RadLabel)e.Item.FindControl("lblsubCategoryLevelId");
                PhoenixRegisterCrewApprovalCategoryLevel.CrewSubCategoryLevelDelete(new Guid(lblsubCategoryLevelId.Text));
             
                gvSubCategoryLevel.Rebind();

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvSubCategoryLevel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdMoreInfo = (ImageButton)e.Item.FindControl("cmdMoreInfo");
            RadLabel lblCongigid = (RadLabel)e.Item.FindControl("lblCongigid");
            if (lblCongigid != null)
                cmdMoreInfo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegisterCrewRoleConfigurationUserList.aspx?ApproverRoleId=" + lblCongigid.Text + "'); return false;");

            RadComboBox ddlConfignameedit = (RadComboBox)e.Item.FindControl("ddlConfignameedit");

            if (ddlConfignameedit != null)
            {
                DataSet ds = PhoenixRegisterCrewApprovalConfiguration.CrewRoleConfigurationList();
                ddlConfignameedit.DataTextField = "FLDCONFIGURATIONNAME";
                ddlConfignameedit.DataValueField = "FLDROLECONFIGURATIONID";
                ddlConfignameedit.DataSource = ds.Tables[0];
                ddlConfignameedit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlConfignameedit.DataBind();
                RadLabel lblCongigidedit = (RadLabel)e.Item.FindControl("lblCongigidedit");
                ddlConfignameedit.SelectedValue = lblCongigidedit.Text;

            }
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlConfignameadd = (RadComboBox)e.Item.FindControl("ddlConfignameadd");

            DataSet ds = PhoenixRegisterCrewApprovalConfiguration.CrewRoleConfigurationList();
            ddlConfignameadd.DataTextField = "FLDCONFIGURATIONNAME";
            ddlConfignameadd.DataValueField = "FLDROLECONFIGURATIONID";
            ddlConfignameadd.DataSource = ds.Tables[0];
            ddlConfignameadd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlConfignameadd.DataBind();
        }

    }

    protected void gvSubCategoryLevel_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace("ASC", "").Replace("DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    public void BindCategory()
    {

        ddlcategory.DataTextField = "FLDAPPROVALCATEGORYNAME";
        ddlcategory.DataValueField = "FLDAPPROVALCATEGORYID";
        ddlcategory.Items.Clear();
        ddlcategory.DataSource = PhoenixRegisterCrewApprovalCategoryLevel.CrewApprovalCategoryList(); ;
        ddlcategory.DataBind();
        
        ddlcategory.AppendDataBoundItems = true;
        ddlcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

    }


    protected void ddlcategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlsubcategory.Items.Clear();
        DataTable dt = PhoenixRegisterCrewApprovalCategoryLevel.CrewApprovalSubCategoryList(General.GetNullableGuid(ddlcategory.SelectedValue.ToString()));
        ddlsubcategory.DataTextField = "FLDAPPROVALSUBCATEGORYNAME";
        ddlsubcategory.DataValueField = "FLDAPPROVALSUBCATEGORYID";
        
        ddlsubcategory.DataSource = dt;
        ddlsubcategory.DataBind();
        ddlsubcategory.AppendDataBoundItems = true;
        ddlsubcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlsubcategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
      
        gvSubCategoryLevel.Rebind();
    }
}
