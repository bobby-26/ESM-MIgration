using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersDocumentCourse : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentCourse.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentCourse')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentCourse.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersDocumentCourse.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Registers/RegistersDocumentCourseList.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOURSE");        
        MenuRegistersDocumentCourse.AccessRights = this.ViewState;
        MenuRegistersDocumentCourse.MenuList = toolbar.Show();        

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Course", "COURSE");
        toolbar.AddButton("Cost of Course", "COSTOFCOURSE");
        MenuCourseCost.AccessRights = this.ViewState;
        MenuCourseCost.MenuList = toolbar.Show();
        MenuCourseCost.SelectedMenuIndex = 0;

        toolbar = new PhoenixToolbar();
        //MenuTitle.AccessRights = this.ViewState;
        //MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            BindChartererList();
            BindDocumentType();
            gvDocumentCourse.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void CourseCost_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("COURSE"))
        {
            Response.Redirect("../Registers/RegistersDocumentCourse.aspx");
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
                                 "FLDGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYN", "FLDPHOTOCOPYACCEPTABLEYN" };
        string[] alCaptions = { "Document Type", "Document Category", "Course", "Active Y/N", "Expiry", "Abbreviation", "Offshore Stage", "Offshore Mandatory Y/N", "Waiver Y/N", 
                                  "User Group to allow Waiver", "Show in 'Additional Documents' on Crew Planner Y/N", "Requires Authentication Y/N",
                                  "Show in Master's checklist onboard Y/N", "Photocopy acceptable Y/N"};

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

        ds = PhoenixRegistersDocumentCourse.DocumentCourseSearch(General.GetNullableInteger(ucDocumentType.SelectedHard), General.GetNullableString(txtSearchCourse.Text),
                                                                chkincludeinactive.Checked==true?null:General.GetNullableInteger("1"), null,
                                                                null, null, null, General.GetNullableString(txtCourseCode.Text),
                                                                null, sortexpression, sortdirection, 1,
                                                                gvDocumentCourse.PageSize, ref iRowCount, ref iTotalPageCount,
                                                                General.GetNullableInteger(ucRank.SelectedRank),
                                                                General.GetNullableInteger(ucVesselType.SelectedVesseltype),
                                                                null
                                                                ,General.GetNullableInteger(ddlCharter.SelectedValue)
                                                                , General.GetNullableInteger(ddldoccategory.SelectedValue)

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

    private void BindChartererList()
    {
        DataSet ds = PhoenixRegistersAddress.ListAddress(((int)PhoenixAddressType.CHARTERER).ToString());
        ddlCharter.DataSource = ds;
        ddlCharter.DataTextField = "FLDNAME";
        ddlCharter.DataValueField = "FLDADDRESSCODE";
        ddlCharter.DataBind();       
    }

    private void BindDocumentType()
    {
        DataTable ds = PhoenixRegistersDocumentCategory.ListDocumentCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null
            , null
            , null);
        ddldoccategory.DataSource = ds;
        ddldoccategory.DataTextField = "FLDCATEGORYNAME";
        ddldoccategory.DataValueField = "FLDDOCUMENTCATEGORYID";
        ddldoccategory.DataBind();
    }

    private void ClearFilter()
    {
        txtSearchCourse.Text = "";
        txtCourseCode.Text = "";
        ucRank.SelectedRank = "";
        ddlCharter.SelectedValue = "";
        ucVesselType.SelectedVesseltype = "";
        ucDocumentType.SelectedName = "";
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvDocumentCourse.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDOCUMENTTYPENAME", "FLDCATEGORYNAME", "FLDCOURSE", "FLDLOCALACTIVEYN", "FLDEXPIRYYN", "FLDABBREVIATION", "FLDSTAGE", "FLDMANDATORYYNNAME", "FLDWAIVERYNNNAME", 
                                 "FLDGROUPNAME", "FLDADDITIONALDOCYNNAME", "FLDAUTHENTICATIONREQYNNAME", "FLDSHOWINMASTERCHECKLISTYN", "FLDPHOTOCOPYACCEPTABLEYN" };
        string[] alCaptions = { "Document Type", "Document Category", "Course", "Active Y/N", "Expiry", "Abbreviation", "Offshore Stage", "Offshore Mandatory Y/N", "Waiver Y/N", 
                                  "User Group to allow Waiver", "Show in 'Additional Documents' on Crew Planner Y/N", "Requires Authentication Y/N",
                                  "Show in Master's checklist onboard Y/N", "Photocopy acceptable Y/N"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string courseSearch = (txtSearchCourse.Text == null) ? "" : txtSearchCourse.Text;

        DataSet ds = PhoenixRegistersDocumentCourse.DocumentCourseSearch(General.GetNullableInteger(ucDocumentType.SelectedHard), General.GetNullableString(txtSearchCourse.Text),
                                                                        chkincludeinactive.Checked == true ? null : General.GetNullableInteger("1"), null,
                                                                        null, null, null, General.GetNullableString(txtCourseCode.Text),
                                                                        null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                                        gvDocumentCourse.PageSize, ref iRowCount, ref iTotalPageCount,
                                                                        General.GetNullableInteger(ucRank.SelectedRank),
                                                                        General.GetNullableInteger(ucVesselType.SelectedVesseltype),
                                                                        null
                                                                        ,General.GetNullableInteger(ddlCharter.SelectedValue)
                                                                        , General.GetNullableInteger(ddldoccategory.SelectedValue)
                                                                        );

        General.SetPrintOptions("gvDocumentCourse", "Course", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDocumentCourse.DataSource = ds;
            gvDocumentCourse.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDocumentCourse.DataSource = "";
        }        
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvDocumentCourse.Rebind();
    }
    

    private void DeleteDocumentCourse(int DocumentId)
    {
        PhoenixRegistersDocumentCourse.DeleteDocumentCourse(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, DocumentId);
    }

    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvDocumentCourse_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;            
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentCourse(Int32.Parse(((RadLabel)e.Item.FindControl("lblDocumentId")).Text));
                BindData();
                gvDocumentCourse.Rebind();
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

    protected void gvDocumentCourse_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentCourse.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvDocumentCourse_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            RadLabel l = (RadLabel)e.Item.FindControl("lblDocumentId");

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkDocumentType");
            if (lb != null) lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersDocumentCourseList.aspx?DocumentCourseId=" + l.Text + "');return false;");
            
            RadLabel lblUserGroup = (RadLabel)e.Item.FindControl("lblUserGroup");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucUserGroup");
            if(uct!=null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblUserGroup.ClientID;
            }
            LinkButton lblRankName = (LinkButton)e.Item.FindControl("lblRankName");
            if(lblRankName!=null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDRANKNAME").ToString() == string.Empty)
                    lblRankName.Visible = false;
            }

            LinkButton lblVesselType = (LinkButton)e.Item.FindControl("lblVesselType");
            if (lblVesselType != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDVESSELTYPE").ToString() == string.Empty)
                    lblVesselType.Visible = false;
            }
            LinkButton lblowner = (LinkButton)e.Item.FindControl("lblowner");
            if (lblowner != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDOWNERLIST").ToString() == string.Empty)
                    lblowner.Visible = false;
            }
            LinkButton lblCompany = (LinkButton)e.Item.FindControl("lblCompany");
            if (lblCompany != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDCOMPANIES").ToString() == string.Empty)
                    lblCompany.Visible = false;
            }
            LinkButton lblFlag = (LinkButton)e.Item.FindControl("lblFlag");
            if (lblFlag != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDFLAG").ToString() == string.Empty)
                    lblFlag.Visible = false;
            }
            LinkButton lblGcomp = (LinkButton)e.Item.FindControl("lblGcomp");
            if (lblGcomp != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDCOMPONENTNAME").ToString() == string.Empty)
                    lblGcomp.Visible = false;
            }
            LinkButton lblwaiveruser = (LinkButton)e.Item.FindControl("lblwaiveruser");
            if (lblwaiveruser != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDGROUPNAME").ToString() == string.Empty)
                    lblwaiveruser.Visible = false;
            }
            LinkButton lblmaker = (LinkButton)e.Item.FindControl("lblmaker");
            if (lblmaker != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDMAKERNAME").ToString() == string.Empty)
                    lblmaker.Visible = false;
            }

            LinkButton lblappvessel = (LinkButton)e.Item.FindControl("lblappvessel");
            if (lblappvessel != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDAPPLICABLEVESSEL").ToString() == string.Empty)
                    lblappvessel.Visible = false;
            }
            //RadLabel lblUserGroup = (RadLabel)e.Item.FindControl("lblUserGroup");
            //LinkButton ImgUserGroup = (LinkButton)e.Item.FindControl("ImgUserGroup");
            //if (ImgUserGroup != null)
            //{
            //    if (lblUserGroup != null)
            //    {
            //        if (lblUserGroup.Text != "")
            //        {
            //            ImgUserGroup.Visible = true;
            //            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucUserGroup");
            //            if (uct != null)
            //            {
            //                ImgUserGroup.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            //                ImgUserGroup.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            //            }
            //        }
            //        else
            //            ImgUserGroup.Visible = false;
            //    }
            //}
        }
    }
               

    protected void gvDocumentCourse_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        //ViewState["SORTEXPRESSION"] = e.SortExpression;
        //switch (e.NewSortOrder)
        //{
        //    case GridSortOrder.Ascending:
        //        ViewState["SORTDIRECTION"] = "0";
        //        break;
        //    case GridSortOrder.Descending:
        //        ViewState["SORTDIRECTION"] = "1";
        //        break;
        //}
    }
}
