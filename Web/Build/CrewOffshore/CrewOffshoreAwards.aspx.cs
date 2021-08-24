using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewOffshoreAwards : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            TabStrip1.AccessRights = this.ViewState;
            TabStrip1.MenuList = toolbar1.Show();


            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreAwards.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvAwardAndCertificate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewAwardandCertificate.AccessRights = this.ViewState;
            CrewAwardandCertificate.MenuList = toolbargrid.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBERCERT"] = 1;
                ViewState["SORTEXPRESSIONCERT"] = null;
                ViewState["SORTDIRECTIONCERT"] = null;
                ViewState["CURRENTINDEXCERT"] = 1;
                ViewState["empid"] = "";

                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != "")
                    ViewState["empid"] = Request.QueryString["empid"].ToString();

                SetEmployeePrimaryDetails();
                gvAwardAndCertificate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindAwardCertificate();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));
            if (dt.Rows.Count > 0)
            {

                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindAwardCertificate()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROWNUMBER", "FLDCERTIFICATENAME", "FLDISSUEDATE", "FLDREMARKS" };
            string[] alCaptions = { "Sl No", "Award/Certificate", "Issue Date", "Remarks" };


            string sortexpression = (ViewState["SORTEXPRESSIONCERT"] == null) ? null : (ViewState["SORTEXPRESSIONCERT"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONCERT"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCERT"].ToString());


            DataSet ds = PhoenixCrewAwardAndCertificate.CrewAwardAndCertificateSearch(Convert.ToInt32(ViewState["empid"].ToString())
                     , sortexpression, sortdirection, (int)ViewState["PAGENUMBERCERT"], gvAwardAndCertificate.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvAwardAndCertificate", "Award and Certificate", alCaptions, alColumns, ds);

            gvAwardAndCertificate.DataSource = ds.Tables[0];
            gvAwardAndCertificate.VirtualItemCount = iRowCount;



            ViewState["ROWCOUNTCERT"] = iRowCount;
            ViewState["TOTALPAGECOUNTCERT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindAwardCertificate();

    }

    protected void AwardandCertificateMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDROWNUMBER", "FLDCERTIFICATENAME", "FLDISSUEDATE", "FLDREMARKS" };
                string[] alCaptions = { "Sl No", "Award/Certificate", "Issue Date", "Remarks" };


                string sortexpression = (ViewState["SORTEXPRESSIONCERT"] == null) ? null : (ViewState["SORTEXPRESSIONCERT"].ToString());
                int? sortdirection = null;

                if (ViewState["SORTDIRECTIONCERT"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCERT"].ToString());


                if (ViewState["ROWCOUNTCERT"] == null || Int32.Parse(ViewState["ROWCOUNTCERT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNTCERT"].ToString());

                DataSet ds = PhoenixCrewAwardAndCertificate.CrewAwardAndCertificateSearch(Convert.ToInt32(ViewState["empid"].ToString())
                     , sortexpression, sortdirection, (int)ViewState["PAGENUMBERCERT"], iRowCount, ref iRowCount, ref iTotalPageCount);

                General.ShowExcel("Award and Certificate", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    private bool IsValidCertificate(string certificate, string issuedate, string remarks)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(certificate) == null)
            ucError.ErrorMessage = "Award/Certificate is required";

        if (General.GetNullableDateTime(issuedate) == null)
            ucError.ErrorMessage = "Issue Date is required";

        if (remarks.Trim() == "")
            ucError.ErrorMessage = "Remarks is required";

        return (!ucError.IsError);
    }


  
   

    protected void gvAwardAndCertificate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAwardAndCertificate.CurrentPageIndex + 1;
            BindAwardCertificate();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAwardAndCertificate_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {

             
                string certificate = ((UserControlQuick)e.Item.FindControl("ddlCertificateAdd")).SelectedValue;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;

                if (!IsValidCertificate(certificate, dateofissue, remarks))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewAwardAndCertificate.InsertCrewAwardAndCertificate(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , Convert.ToInt32(certificate)
                    , General.GetNullableDateTime(dateofissue)
                    , General.GetNullableString(remarks)
                    , Convert.ToInt32(ViewState["empid"].ToString())
                    );

                BindAwardCertificate();
                gvAwardAndCertificate.Rebind();

            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if(e.CommandName.ToUpper()=="UPDATE")
            {
                
                try
                {
                    string awardid = ((RadLabel)e.Item.FindControl("lblAwardIdEdit")).Text;
                    string certificate = ((UserControlQuick)e.Item.FindControl("ddlCertificateEdit")).SelectedValue;
                    string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
                    string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;

                    if (!IsValidCertificate(certificate, dateofissue, remarks))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixCrewAwardAndCertificate.UpdateCrewAwardAndCertificate(
                              PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Convert.ToInt32(awardid)
                            , Convert.ToInt32(certificate)
                            , General.GetNullableDateTime(dateofissue)
                            , General.GetNullableString(remarks)
                            , Convert.ToInt32(ViewState["empid"].ToString())
                            );

                    
                    BindAwardCertificate();
                    gvAwardAndCertificate.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "Please make the required correction";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;

                }
            }
            if(e.CommandName.ToUpper()=="DELETE")
            {
                try
                {
                   
                    string awardid = ((RadLabel)e.Item.FindControl("lblAwardId")).Text;

                    PhoenixCrewAwardAndCertificate.DeleteCrewAwardAndCertificate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            Convert.ToInt32(awardid));
                    BindAwardCertificate();
                    gvAwardAndCertificate.Rebind();

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;

                }
            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvAwardAndCertificate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
               && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdXDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                    RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                    LinkButton att = (LinkButton)e.Item.FindControl("cmdXAtt");
                    if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
                    LinkButton cme = (LinkButton)e.Item.FindControl("cmdXEdit");
                    if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                    RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
                    HtmlGenericControl html = new HtmlGenericControl();
                    if (lblIsAtt.Text == string.Empty)
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color: gray;\"><i class=\"fas fa-paperclip\"></i></span>";
                        att.Controls.Add(html);
                        //att.ImageUrl = Session["images"] + "/no-attachment.png";
                    }
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ACADEMICS + "&cmdname=AWARDANDCERTIFICATE'); return false;");

                    RadLabel lblawardid = (RadLabel)e.Item.FindControl("lblAwardId");
                }
            }
            UserControlQuick ddlcertificate = (UserControlQuick)e.Item.FindControl("ddlCertificateEdit");
            DataRowView drvCertificate = (DataRowView)e.Item.DataItem;
            if (ddlcertificate != null) ddlcertificate.SelectedValue = drvCertificate["FLDCERTIFICATE"].ToString();
        }
    }
}
