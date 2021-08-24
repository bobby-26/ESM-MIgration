using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class RegistersOffshoreDocumentCourse : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersOffshoreDocumentCourse.aspx", "Export to Excel","<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentCourse')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");              
        toolbar.AddFontAwesomeButton("../Registers/RegistersOffshoreDocumentCourse.aspx","Find","<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersOffshoreDocumentCourse.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Registers/RegistersDocumentCourseList.aspx')" , "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

        MenuRegistersDocumentCourse.AccessRights = this.ViewState;
        MenuRegistersDocumentCourse.MenuList = toolbar.Show();
        
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");           

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Course", "COURSE");
        toolbar.AddButton("Cost of Course", "COSTOFCOURSE");
        MenuCourseCost.AccessRights = this.ViewState;
        MenuCourseCost.MenuList = toolbar.Show();
        MenuCourseCost.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvDocumentCourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        BindData();
    }

    protected void CourseCost_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;

        if (CommandName.ToUpper().Equals("COURSE"))
        {
            Response.Redirect("../Registers/RegistersOffshoreDocumentCourse.aspx");
        }
        else if (CommandName.ToUpper().Equals("COSTOFCOURSE"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCourseCost.aspx");
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDOCUMENTTYPENAME", "FLDCATEGORYNAME", "FLDCOURSE", "FLDLOCALACTIVEYN", "FLDEXPIRYYN", "FLDABBREVIATION", "FLDSTAGE", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", 
                                 "FLDGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYN", "FLDPHOTOCOPYACCEPTABLEYN","FLDSHOWINSUBCATEGORYYN" };
        string[] alCaptions = { "Document Type", "Document Category", "Course", "Active Y/N", "Expiry", "Abbreviation", "Offshore Stage", "Offshore Mandatory Y/N", "Waiver Y/N", 
                                  "User Group to allow Waiver", "Show in 'Additional Documents' on Crew Planner Y/N", "Requires Authentication Y/N",
                                  "Show in Master's checklist onboard Y/N", "Photocopy acceptable Y/N","Map in Competence subcategory Y/N"};

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //string courseSearch = (txtSearchCourse.Text == null) ? "" : txtSearchCourse.Text;

        ds = PhoenixRegistersDocumentCourse.OffshoreDocumentCourseSearch(General.GetNullableInteger(ucDocumentType.SelectedHard), General.GetNullableString(txtSearchCourse.Text), null, null,
                                                                null, null, null, General.GetNullableString(txtCourseCode.Text),
                                                                null, sortexpression, sortdirection, 1,
                                                                gvDocumentCourse.PageSize, ref iRowCount, ref iTotalPageCount,
                                                                General.GetNullableInteger(ucRank.SelectedRank),
                                                                General.GetNullableInteger(ucVesselType.SelectedVesseltype),
                                                                General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentCourse.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document Course</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersDocumentCourse_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvDocumentCourse.Rebind();
           
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
            ViewState["PAGENUMBER"] = 1;
            ClearFilter();
            BindData();
            gvDocumentCourse.Rebind();
           
        }
    }

    private void ClearFilter()
    {
        txtSearchCourse.Text = "";
        txtCourseCode.Text = "";
        ucRank.SelectedRank = "";
        ucVessel.SelectedVessel = "Dummy";
        ucVesselType.SelectedVesseltype = "";
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDOCUMENTTYPENAME", "FLDCATEGORYNAME", "FLDCOURSE", "FLDLOCALACTIVEYN", "FLDEXPIRYYN", "FLDABBREVIATION", "FLDSTAGE", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", 
                                 "FLDGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYN", "FLDPHOTOCOPYACCEPTABLEYN","FLDSHOWINSUBCATEGORYYN" };
        string[] alCaptions = { "Document Type", "Document Category", "Course", "Active Y/N", "Expiry", "Abbreviation", "Offshore Stage", "Offshore Mandatory Y/N", "Waiver Y/N", 
                                  "User Group to allow Waiver", "Show in 'Additional Documents' on Crew Planner Y/N", "Requires Authentication Y/N",
                                  "Show in Master's checklist onboard Y/N", "Photocopy acceptable Y/N","Map in Competence subcategory Y/N"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string courseSearch = (txtSearchCourse.Text == null) ? "" : txtSearchCourse.Text;

        DataSet ds = PhoenixRegistersDocumentCourse.OffshoreDocumentCourseSearch(General.GetNullableInteger(ucDocumentType.SelectedHard), General.GetNullableString(txtSearchCourse.Text), null, null,
                                                                        null, null, null, General.GetNullableString(txtCourseCode.Text),
                                                                        null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                                       gvDocumentCourse.PageSize, ref iRowCount, ref iTotalPageCount,
                                                                        General.GetNullableInteger(ucRank.SelectedRank),
                                                                        General.GetNullableInteger(ucVesselType.SelectedVesseltype),
                                                                        General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                        );

        General.SetPrintOptions("gvDocumentCourse", "Course", alCaptions, alColumns, ds);

        gvDocumentCourse.DataSource = ds;
        gvDocumentCourse.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvDocumentCourse_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersDocumentCourse.DeleteDocumentCourse(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentId")).Text)));
                BindData();
                gvDocumentCourse.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
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

    protected void gvDocumentCourse_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);


            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");


                RadLabel l = (RadLabel)e.Item.FindControl("lblDocumentId");

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkDocumentType");

                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Registers/RegistersDocumentCourseList.aspx?DocumentCourseId=" + l.Text + "');return false;");
            
            //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))

            RadLabel lblUserGroup = (RadLabel)e.Item.FindControl("lblUserGroup");
            LinkButton ImgUserGroup = (LinkButton)e.Item.FindControl("ImgUserGroup");
            if (ImgUserGroup != null)
            {
                if (lblUserGroup != null)
                {
                    if (lblUserGroup.Text != "")
                    {
                        ImgUserGroup.Visible = true;
                        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucUserGroup");
                        if (uct != null)
                        {
                            uct.Position = ToolTipPosition.TopCenter;
                            uct.TargetControlId = ImgUserGroup.ClientID;
                        }
                    }
                    else
                        ImgUserGroup.Visible = false;
                }
            }
        }
    }

    protected void gvDocumentCourse_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

     protected void gvDocumentCourse_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentCourse.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
