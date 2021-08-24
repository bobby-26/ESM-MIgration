using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersBatch : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersBatch.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBatch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersBatch.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersBatchList.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDBATCH");            
            MenuRegistersBatch.AccessRights = this.ViewState;
            MenuRegistersBatch.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvBatch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvBatch.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDINSTITUTIONNAME", "FLDCOURSENAME", "FLDBATCH", "FLDFROMDATE", "FLDTODATE", "FLDNOOFSEATS", "FLDDURATION" };
            string[] alCaptions = { "Institution", "Course", "Batch", "From Date", "To Date", "No Of Seats", "Duration" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixRegistersBatch.BatchSearch(
                        null
                        , null, General.GetNullableString(txtBatch.Text)
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , gvBatch.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount, null, null, null, General.GetNullableInteger(ddlType.SelectedValue));

            General.SetPrintOptions("gvBatch", "Batch", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBatch.DataSource = ds;
                gvBatch.VirtualItemCount = iRowCount;
            }
            else
            {
                gvBatch.DataSource = "";
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Training_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;      
        if (CommandName.ToUpper().Equals("POSTSEA"))
        {
            //Response.Redirect("../Registers/RegistersTrainingStaff.aspx?departmentid=" + ViewState["deptid"].ToString(), false);
        }

    }
    protected void RegistersBatch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvBatch.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDINSTITUTIONNAME", "FLDCOURSENAME", "FLDBATCH", "FLDFROMDATE", "FLDTODATE", "FLDNOOFSEATS", "FLDDURATION" };
                string[] alCaptions = { "Institution", "Course", "Batch", "From Date", "To Date", "No Of Seats", "Duration" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                DataSet ds = PhoenixRegistersBatch.BatchSearch(
                                        null
                                        , null, null
                                        , sortexpression, sortdirection
                                        , (int)ViewState["PAGENUMBER"]
                                        , iRowCount
                                        , ref iRowCount
                                        , ref iTotalPageCount, null, null, null, General.GetNullableInteger(ddlType.SelectedValue));

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Batch", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvBatch_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string batchid = ((RadLabel)e.Item.FindControl("lblBatchId")).Text;
                PhoenixRegistersBatch.DeleteBatch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(batchid));
                BindData();
                gvBatch.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBatch_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            RadLabel l = (RadLabel)e.Item.FindControl("lblBatchId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCourseName");
            lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersBatchList.aspx?batchid=" + l.Text + "');return false;");

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            db1.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Registers/RegistersBatchList.aspx?batchid=" + l.Text + "');return false;");

            //UserControlAddressType ucAddressType = (UserControlAddressType)e.Item.FindControl("ucInstitutionEdit");
            //DataRowView drv = (DataRowView)e.Item.DataItem;
            //if (ucAddressType != null) ucAddressType.SelectedAddress = drv["FLDINSTITUTIONID"].ToString();

            UserControlCourse ucCourse = (UserControlCourse)e.Item.FindControl("ddlCourseEdit");
            DataRowView drvCourse = (DataRowView)e.Item.DataItem;
            if (ucCourse != null) ucCourse.SelectedCourse = drvCourse["FLDCOURSE"].ToString();
        }
    }

    protected void gvBatch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBatch.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvBatch_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
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
}
