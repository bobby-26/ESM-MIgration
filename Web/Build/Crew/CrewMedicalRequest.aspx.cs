using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewMedicalRequest : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Crew/CrewMedicalRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvReq')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','"+Session["sitepath"]+"/Crew/CrewMedicalRequestFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewMedicalRequest.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            //toolbarsub.AddImageLink("javascript:parent.Openpopup('codehelp1','','CrewMedicalRequestInitiate.aspx')", "Add", "add.png", "ADD");
            MRMenu.AccessRights = this.ViewState;
            MRMenu.MenuList = toolbarsub.Show();
            if (!Page.IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["REQUESTTYPE"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 121, "ACD");
                ViewState["TESTCONDUCTED"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 121, "EPO");
                ViewState["PAID"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 121, "CBP");
                ViewState["PAIDBYEMP"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 121, "PBS");
                ViewState["CANCEL"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 121, "CNC");

                gvReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MRMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
               
                Filter.CurrentMedicalRequestFilter = null;
                ViewState["PAGENUMBER"] = 1;
                gvReq.CurrentPageIndex = 0;
                gvReq.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDREFNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELCODE", "FLDJOININGVESSELCODE", "FLDCLINICADDRESSNAME", "FLDSEAFARERNAME", "FLDINVOICENO", "FLDMEDICALCOST", "FLDCREATEDDATE", "FLDCREATEDBY", "FLDHARDNAME" };
                string[] alCaptions = { "Request No", "File No", "Name", "Rank", "Tentative Vessel", "Joined Vessel", "Clinic", "Family Member", "Invoice No", "Cost", "Created On", "Created By", "Status" };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.CurrentMedicalRequestFilter;

                DataTable dt = PhoenixCrewMedical.SearchMedicalRequest(
                                                           (nvc != null ? nvc["txtReqNo"] : string.Empty)
                                                           , (nvc != null ? nvc["txtFileno"] : string.Empty)
                                                           , (nvc != null ? nvc["txtName"] : string.Empty)
                                                           , null, null
                                                           , General.GetNullableInteger(nvc != null ? nvc["ddlClinic"] : string.Empty)
                                                           , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                           , (nvc != null ? General.GetNullableInteger(nvc["chkInactive"]) : 1)
                                                           , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                           , General.GetNullableInteger(nvc != null ? nvc["ddlCreatedBy"] : string.Empty)
                                                           , sortexpression, sortdirection
                                                           , (int)ViewState["PAGENUMBER"], gvReq.PageSize
                                                           , ref iRowCount, ref iTotalPageCount
                                                           , General.GetNullableInteger(nvc != null ? nvc["ucHard"] : string.Empty));

                
                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Medical Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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

        gvReq.Rebind();
    }

 

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREFNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELCODE", "FLDJOININGVESSELCODE", "FLDCLINICADDRESSNAME", "FLDSEAFARERNAME", "FLDINVOICENO", "FLDMEDICALCOST", "FLDCREATEDDATE", "FLDCREATEDBY", "FLDHARDNAME" };
        string[] alCaptions = { "Request No", "File No", "Name", "Rank", "Tentative Vessel", "Joined Vessel", "Clinic", "Family Member", "Invoice No", "Cost", "Created On", "Created By", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //default desc order
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            NameValueCollection nvc = Filter.CurrentMedicalRequestFilter;

            DataTable dt = PhoenixCrewMedical.SearchMedicalRequest(
                                                            (nvc != null ? nvc["txtReqNo"] : string.Empty)
                                                            , (nvc != null ? nvc["txtFileno"] : string.Empty)
                                                            , (nvc != null ? nvc["txtName"] : string.Empty)
                                                            , null, null
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlClinic"] : string.Empty)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                            , (nvc != null ? General.GetNullableInteger(nvc["chkInactive"]) : 1)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlCreatedBy"] : string.Empty)
                                                            , sortexpression, sortdirection
                                                            , (int)ViewState["PAGENUMBER"], gvReq.PageSize
                                                            , ref iRowCount, ref iTotalPageCount
                                                            , General.GetNullableInteger(nvc != null ? nvc["ucHard"] : string.Empty));


            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvReq", "Medical Request", alCaptions, alColumns, ds);
            gvReq.DataSource = dt;
            gvReq.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidMedicalRequest(string invoiceno)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (invoiceno.Trim() == "")
            ucError.ErrorMessage = "Invoice No is required";

        return (!ucError.IsError);
    }


    protected void gvReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReq.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else if(e.CommandName.ToUpper()=="UPDATE")
        {
           
            try
            {
                string reqid = ((RadLabel)e.Item.FindControl("lblReqId")).Text;
                // string invoiceno = ((TextBox)e.Item.FindControl("txtInvoiceNo")).Text;
                CheckBox chk = ((CheckBox)e.Item.FindControl("chkVerifiedEdit"));
                string verifiedyn = chk.Checked == true ? "1" : "0";
                if (verifiedyn == "1")
                {
                    PhoenixCrewMedical.UpdateMedicalRequest(new Guid(reqid)
                        , byte.Parse(verifiedyn),
                        verifiedyn == "1" ? General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 121, "CBP")) : null
                       );
                }

                string attendedstatus = ((RadLabel)e.Item.FindControl("lblAttendedStatus")).Text;
                string reqtype = ((RadLabel)e.Item.FindControl("lblReqType")).Text;
                CheckBox chkattended = ((CheckBox)e.Item.FindControl("chkAttendedEdit"));
                string attendedyn = chkattended.Checked == true ? "1" : "0";

                if (chkattended.Checked == true && attendedstatus == "")
                {
                    PhoenixCrewMedical.UpdateMedicalRequestCost(new Guid(reqid), General.GetNullableInteger(reqtype), byte.Parse(attendedyn));
                }

                CheckBox chkreceived = ((CheckBox)e.Item.FindControl("chkReceivedEdit"));
                string receivedyn = chkreceived.Checked == true ? "1" : "0";

                if (chkreceived.Checked == true && reqtype == ViewState["TESTCONDUCTED"].ToString())
                {
                    PhoenixCrewMedical.UpdateMedicalRequestReceived(new Guid(reqid), byte.Parse(receivedyn));
                }


            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
           
            gvReq.Rebind();
        }

    }

    protected void gvReq_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton med = (LinkButton)e.Item.FindControl("cmdMedical");
        if (med != null) med.Visible = SessionUtil.CanAccess(this.ViewState, med.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton md = (LinkButton)e.Item.FindControl("cmdMedical");
            //ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (md != null) md.Attributes.Add("onclick", "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&emailyn=1&reportcode=MEDICALSLIP&reqid=" + drv["FLDREQUESTID"].ToString() + "'); return false;");

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkEmployee");
            LinkButton lnkInvoiceNo = (LinkButton)e.Item.FindControl("lnkInvoiceNo");
            if (drv["FLDNEWAPP"].ToString() == "1")
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&med=1'); return false;");
            }
            else
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&med=1'); return false;");
            }

            LinkButton lbFamily = (LinkButton)e.Item.FindControl("lnkFamilyEmployee");
            if (drv["FLDNEWAPP"].ToString() == "1")
            {
                lbFamily.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewFamilyMedicalDocument.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");

            }
            else
            {
                lbFamily.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewFamilyMedicalDocument.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");
            }



            if (lnkInvoiceNo != null)
            {
                lnkInvoiceNo.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDINVOICEDTKEY"].ToString() + "&mod="
                      + PhoenixModule.ACCOUNTS + "&type=Invoice&U=0'); return false;");
            }

            if (drv["FLDREQUESTTYPE"].ToString() == ViewState["PAID"].ToString() || drv["FLDREQUESTTYPE"].ToString() == ViewState["PAIDBYEMP"].ToString() || drv["FLDREQUESTTYPE"].ToString() == ViewState["CANCEL"].ToString())
            {
                edit.Visible = false;
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

        }

    }

    protected void gvReq_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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
