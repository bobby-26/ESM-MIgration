using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewCommon;
using Telerik.Web.UI;
public partial class Crew_CrewLicenceRequestDetailList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "BACK");
            toolbar.AddButton("Licence Detail", "REQUEST");
            toolbar.AddButton("Payment", "PAYMENT");
            CrewLicReq.AccessRights = this.ViewState;
            CrewLicReq.MenuList = toolbar.Show();
            CrewLicReq.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarLicence = new PhoenixToolbar();
            toolbarLicence = new PhoenixToolbar();
            toolbarLicence.AddButton("Refresh", "REFRESH", ToolBarDirection.Right);
            MenuLicence.AccessRights = this.ViewState;
            MenuLicence.MenuList = toolbarLicence.Show();

            if (!Page.IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                ViewState["FLAGID"] = string.Empty;
                ViewState["REQUESTID"] = null;
                if (Request.QueryString["rid"] != null && Request.QueryString["rid"].ToString() != string.Empty)
                    ViewState["REQUESTID"] = Request.QueryString["rid"].ToString();

                SetEmployeeDetails();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

            }

            BindNatData();
            BindFlagData();
            BindDCEData();
            BindSeamansBookData();
            BindCourseData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void CrewLicReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("../Crew/CrewLicenceRequestDetailList.aspx?rid=" + ViewState["REQUESTID"].ToString());
            }
            if (CommandName.ToUpper().Equals("PAYMENT"))
            {
                Response.Redirect("../Crew/CrewLicenceRequestPayment.aspx?rid=" + ViewState["REQUESTID"].ToString());
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewLicenceRequestList.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLicence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("REFRESH"))
            {

                if (string.IsNullOrEmpty(txtDate.Text))
                {
                    ucError.ErrorMessage = "Enter Correct Date format";
                    ucError.Visible = true;
                    return;
                }

                ViewState["Date"] = txtDate.Text;
                BindNatData();
                BindFlagData();
                BindDCEData();
                BindSeamansBookData();
                BindCourseData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void SetEmployeeDetails()
    {
        try
        {
            if (ViewState["REQUESTID"] != null)
            {
                DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceRequest(new Guid(ViewState["REQUESTID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtReferenceNo.Text = dt.Rows[0]["FLDREQUISITIONNUMBER"].ToString();
                    txtEmployeeName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
                    txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();
                    txtFlag.Text = dt.Rows[0]["FLDFLAGNAME"].ToString();
                    txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                    ViewState["Empid"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                    ViewState["Date"] = dt.Rows[0]["FLDCREWCHANGEDATE"].ToString();
                    txtDate.Text = dt.Rows[0]["FLDCREWCHANGEDATE"].ToString();
                    ViewState["Rankid"] = dt.Rows[0]["FLDRANKID"].ToString();
                    txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                    txtConsulate.Text = dt.Rows[0]["FLDCONSULATENAME"].ToString();
                    ViewState["Vesselid"] = dt.Rows[0]["FLDVESSELID"].ToString();
                    ViewState["FLAGID"] = dt.Rows[0]["FLDFLAGID"].ToString();

                }
            }
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

        BindCourseData();
        gvCourse.Rebind();
        BindDCEData();
        gvDCE.Rebind();
        BindFlagData();
        gvFlag.Rebind();
        BindSeamansBookData();
        gvSeamanBook.Rebind();

    }


    protected void ddlDocument_TextChanged(object sender, EventArgs e)
    {
        UserControlDocuments dc = (UserControlDocuments)sender;
        DataSet ds = new DataSet();
        UserControlDate issuedate;
        UserControlDate expirydate;
        if (dc.SelectedDocument != "Dummy")
        {
            ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(General.GetNullableInteger(dc.SelectedDocument).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlLicenceAdd" || dc.ID == "ddlNatLicenceAdd")
            {
                GridFooterItem Item = (GridFooterItem)dc.NamingContainer;
                issuedate = (UserControlDate)Item.FindControl("txtIssueDateAdd");
                expirydate = (UserControlDate)Item.FindControl("txtExpiryDateAdd");
                RadTextBox txtLicenceNumberAdd = (RadTextBox)Item.FindControl("txtLicenceNumberAdd");
            }
            else
            {
                GridDataItem dataItem = (GridDataItem)dc.NamingContainer;
                issuedate = (UserControlDate)dataItem.FindControl("txtIssueDateEdit");
                expirydate = (UserControlDate)dataItem.FindControl("txtExpiryDateEdit");
                RadTextBox txtLicenceNumberEdit = (RadTextBox)dataItem.FindControl("txtLicenceNumberEdit");
                txtLicenceNumberEdit.Focus();
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                issuedate.CssClass = "input_mandatory";
                expirydate.CssClass = "input_mandatory";
            }
            else
            {
                issuedate.CssClass = "";
                expirydate.CssClass = "";
            }
        }
    }

    protected void gvMissingLicence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindNatData();
    }


    protected void BindNatData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.NationalLicenceMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate));

            gvMissingLicence.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMissingLicence_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");

            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }
        }
    }

    protected void BindFlagData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.FlagLicenceMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate), General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            gvFlag.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvFlag_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindFlagData();
    }

    protected void gvFlag_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");

            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                       + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&cmdname=ENDORSEMENTUPLOAD'); return false;");
            }

            RadLabel lblRequestDetailId = (RadLabel)e.Item.FindControl("lblRequestDetailId");
            if (lblRequestDetailId != null)
            {
                if (lblRequestDetailId.Text == string.Empty)
                {
                    if (cme != null) cme.Visible = false;
                    if (db != null) db.Visible = false;
                    if (att != null) att.Visible = false;
                }
            }

            RadLabel lbtn1 = (RadLabel)e.Item.FindControl("lblConnectedToVessel");
            UserControlToolTip uct1 = (UserControlToolTip)e.Item.FindControl("ucToolTipConnectedToVessel");
            if (lbtn1 != null)
            {
                lbtn1.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct1.ToolTip + "', 'visible');");
                lbtn1.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct1.ToolTip + "', 'hidden');");
            }
        }

        if (e.Item.IsInEditMode)
        {

            UserControlDocuments ucNatDocuments = (UserControlDocuments)e.Item.FindControl("ddlNatLicenceEdit");
            DataRowView drvNatDocuments = (DataRowView)e.Item.DataItem;
            if (ucNatDocuments != null) ucNatDocuments.SelectedDocument = drvNatDocuments["FLDDOCUMENTID"].ToString();

            UserControlDocuments ucFlagLicence = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
            DataRowView drvFlagLicence = (DataRowView)e.Item.DataItem;
            if (ucFlagLicence != null)
            {
                ucFlagLicence.SelectedDocument = "";
                ucFlagLicence.SelectedDocument = drvFlagLicence["FLDFLAGDOCUMENTID"].ToString();
            }

            UserControlFlag ucFlag = (UserControlFlag)e.Item.FindControl("ddlFlagEdit");
            DataRowView drvFlag = (DataRowView)e.Item.DataItem;
            if (ucFlag != null) ucFlag.SelectedFlag = drvFlag["FLDFLAGID"].ToString();

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvFlag_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                string requestid = ViewState["REQUESTID"].ToString();
                string Natlicence = ((UserControlDocuments)e.Item.FindControl("ddlNatLicenceAdd")).SelectedDocument;
                string Flaglicence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceAdd")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagAdd"));
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByAdd")).Text;
                string connectedtovessel = ((CheckBox)e.Item.FindControl("chkConnectedToVesselAdd")).Checked ? "1" : "0";

                if (!IsValidLicenceFlagEndorsement(requestid, Flaglicence, Natlicence, licencenumber, placeofissue, dateofissue, dateofexpiry, ddlFlag.SelectedFlag))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewLicenceRequest.InsertLicenceRequestFlagEndorsement(
                                            new Guid(requestid)
                                            , Convert.ToInt32(Natlicence)
                                            , Convert.ToInt32(Flaglicence)
                                            , licencenumber
                                            , General.GetNullableDateTime(dateofissue).Value
                                            , placeofissue
                                            , General.GetNullableDateTime(dateofexpiry.Text)
                                            , General.GetNullableInteger(ddlFlag.SelectedFlag).Value
                                            , byte.Parse("0")
                                            , issuingauthority
                                            , byte.Parse(connectedtovessel)
                                           );

                BindFlagData();
                gvFlag.Rebind();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvFlag_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailIdEdit")).Text;
            string Natlicence = ((UserControlDocuments)e.Item.FindControl("ddlNatLicenceEdit")).SelectedDocument;
            string Flaglicence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceEdit")).SelectedDocument;
            string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberEdit")).Text;
            string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
            UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
            UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagEdit"));
            string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByEdit")).Text;

            CheckBox chk1 = ((CheckBox)e.Item.FindControl("chkConnectedtoVesselEdit"));
            string connectedtovessel;
            if (chk1.Enabled == true)
                connectedtovessel = chk1.Checked && chk1.Enabled ? "1" : "0";
            else
                connectedtovessel = "1";

            if (!IsValidLicenceFlagEndorsement(requestdetailid, Natlicence, Flaglicence, licencenumber, placeofissue, dateofissue, dateofexpiry, ddlFlag.SelectedFlag))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateLicenceRequestFlagEndorsement(
                                            new Guid(requestdetailid)
                                            , Convert.ToInt32(Natlicence)
                                            , Convert.ToInt32(Flaglicence)
                                            , licencenumber
                                            , General.GetNullableDateTime(dateofissue).Value
                                            , placeofissue
                                            , General.GetNullableDateTime(dateofexpiry.Text)
                                            , General.GetNullableInteger(ddlFlag.SelectedFlag).Value
                                            , 0
                                            , issuingauthority
                                            , byte.Parse(connectedtovessel)
                                           );
            BindFlagData();
            gvFlag.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFlag_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailId")).Text;

            if (requestdetailid == "")
            {
                ucError.ErrorMessage = "Cannot Delete. Not a valid request document";
                ucError.Visible = true;
            }
            else
            {
                PhoenixCrewLicenceRequest.DeleteLicenceRequestDocument(new Guid(requestdetailid));
            }

            BindFlagData();
            gvFlag.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlFEDocument_TextChanged(object sender, EventArgs e)
    {
        UserControlDocuments dc = (UserControlDocuments)sender;
        DataSet ds = new DataSet();
        UserControlDate expirydate;
        if (dc.SelectedDocument != "Dummy")
        {
            ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(General.GetNullableInteger(dc.SelectedDocument).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlLicenceAdd")
            {
                GridFooterItem Item = (GridFooterItem)dc.NamingContainer;
                expirydate = (UserControlDate)Item.FindControl("txtExpiryDateAdd");
                RadTextBox txtLicenceNumberAdd = (RadTextBox)Item.FindControl("txtLicenceNumberAdd");
            }
            else
            {
                GridDataItem dataItem = (GridDataItem)dc.NamingContainer;
                expirydate = (UserControlDate)dataItem.FindControl("txtExpiryDateEdit");
                RadTextBox txtLicenceNumberEdit = (RadTextBox)dataItem.FindControl("txtLicenceNumberEdit");
                txtLicenceNumberEdit.Focus();
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                expirydate.CssClass = "input_mandatory";
            }
            else
            {
                expirydate.CssClass = "input";
            }
        }
    }

    private bool IsValidLicenceFlagEndorsement(string requestdetailid, string natlicence, string licence, string licencenumber, string placeofissue, string dateofissue, UserControlDate dateofexpiry, string flag)
    {
        int resultInt;
        DateTime resultDate;
        DateTime dtExpiryDate = DateTime.Now;
        DateTime dtIddueDate = DateTime.Now;

        ucError.HeaderMessage = "Please provide the following required information";

        if (requestdetailid.Trim() == "")
            ucError.ErrorMessage = "Selected Licence Not Requested";
        else
        {
            if (!int.TryParse(licence, out resultInt))
                ucError.ErrorMessage = "Licence is required";

            if (!int.TryParse(natlicence, out resultInt))
                ucError.ErrorMessage = "National Licence is required";

            if (licencenumber.Trim() == "")
                ucError.ErrorMessage = "Licence Number is required";

            if (placeofissue.Trim() == "")
                ucError.ErrorMessage = "Place of Issue is required";

            if (!DateTime.TryParse(dateofissue, out resultDate))
                ucError.ErrorMessage = "Issue Date is required.";
            else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Issue Date should be earlier than current date";
            }

            if (!DateTime.TryParse(dateofexpiry.Text, out resultDate) && dateofexpiry.CssClass == "input_mandatory")
                ucError.ErrorMessage = "Expiry Date is required.";

            if (!int.TryParse(flag, out resultInt))
                ucError.ErrorMessage = "Flag is required";
        }

        return (!ucError.IsError);
    }


    protected void gvDCE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDCEData();
    }

    protected void BindDCEData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.DCEMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate), General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            gvDCE.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvDCE_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");

            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                  + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&cmdname=DCEUPLOAD'); return false;");
            }

            RadLabel lblRequestDetailId = (RadLabel)e.Item.FindControl("lblRequestDetailId");
            if (lblRequestDetailId != null)
            {
                if (lblRequestDetailId.Text == string.Empty)
                {
                    if (cme != null) cme.Visible = false;
                    if (db != null) db.Visible = false;
                    if (att != null) att.Visible = false;
                }
            }

            RadLabel lbtn1 = (RadLabel)e.Item.FindControl("lblDCEConnectedToVessel");
            UserControlToolTip uct1 = (UserControlToolTip)e.Item.FindControl("ucToolTipDCEConnectedToVessel");
            if (lbtn1 != null)
            {
                lbtn1.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct1.ToolTip + "', 'visible');");
                lbtn1.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct1.ToolTip + "', 'hidden');");
            }
        }

        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlDocuments ucDocuments = (UserControlDocuments)e.Item.FindControl("ddlLicenceEdit");
            if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDDOCUMENTID"].ToString();

            UserControlFlag ucFlag = (UserControlFlag)e.Item.FindControl("ddlFlagEdit");
            if (ucFlag != null) ucFlag.SelectedFlag = drv["FLDFLAGID"].ToString();

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvDCE_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                string requestid = ViewState["REQUESTID"].ToString();
                string empid = ViewState["Empid"].ToString();
                string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceAdd")).SelectedDocument;
                string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberAdd")).Text;
                string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateAdd")).Text;
                string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueAdd")).Text;
                UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateAdd"));
                UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagAdd"));
                string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByAdd")).Text;
                string connectedtovessel = ((CheckBox)e.Item.FindControl("chkDCEConnectedToVesselAdd")).Checked ? "1" : "0";

                if (!IsValidLicence(requestid, licence, licencenumber, placeofissue, dateofissue, dateofexpiry, ddlFlag.SelectedFlag))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewLicenceRequest.InsertLicenceRequestDCE(
                                          new Guid(requestid)
                                          , Convert.ToInt32(empid)
                                          , Convert.ToInt32(licence)
                                          , licencenumber
                                          , General.GetNullableDateTime(dateofissue).Value
                                          , placeofissue
                                          , General.GetNullableDateTime(dateofexpiry.Text)
                                          , General.GetNullableInteger(ddlFlag.SelectedFlag).Value
                                          , byte.Parse("0")
                                          , issuingauthority
                                          , byte.Parse(connectedtovessel)
                                         );
                BindDCEData();
                gvDCE.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDCE_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailIdEdit")).Text;
            string empid = ViewState["Empid"].ToString();
            string licence = ((UserControlDocuments)e.Item.FindControl("ddlLicenceEdit")).SelectedDocument;
            string licencenumber = ((RadTextBox)e.Item.FindControl("txtLicenceNumberEdit")).Text;
            string dateofissue = ((UserControlDate)e.Item.FindControl("txtIssueDateEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssueEdit")).Text;
            UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("txtExpiryDateEdit"));
            string licenceid = ((RadLabel)e.Item.FindControl("lblLicenceIdEdit")).Text;
            UserControlFlag ddlFlag = ((UserControlFlag)e.Item.FindControl("ddlFlagEdit"));

            string issuingauthority = ((RadTextBox)e.Item.FindControl("txtIssuedByEdit")).Text;

            CheckBox chk1 = ((CheckBox)e.Item.FindControl("chkDCEConnectedtoVesselEdit"));
            string connectedtovessel;
            if (chk1.Enabled == true)
                connectedtovessel = chk1.Checked && chk1.Enabled ? "1" : "0";
            else
                connectedtovessel = "1";

            if (!IsValidLicence(requestdetailid, licence, licencenumber, placeofissue, dateofissue, dateofexpiry, ddlFlag.SelectedFlag))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewLicenceRequest.UpdateLicenceRequestDCE(
                                           new Guid(requestdetailid)
                                           , Convert.ToInt32(empid)
                                            , Convert.ToInt32(licence)
                                           , licencenumber
                                           , General.GetNullableDateTime(dateofissue).Value
                                           , placeofissue
                                           , General.GetNullableDateTime(dateofexpiry.Text)
                                           , General.GetNullableInteger(ddlFlag.SelectedFlag).Value
                                           , byte.Parse("0")
                                           , issuingauthority
                                           , byte.Parse(connectedtovessel)
                                           );

            BindDCEData();
            gvDCE.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void gvDCE_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailId")).Text;

            if (requestdetailid == "")
            {
                ucError.ErrorMessage = "Cannot Delete. Not a valid request document";
                ucError.Visible = true;
            }
            else
            {
                PhoenixCrewLicenceRequest.DeleteLicenceRequestDocument(new Guid(requestdetailid));
            }

            BindDCEData();
            gvDCE.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlDCEDocument_TextChanged(object sender, EventArgs e)
    {
        UserControlDocuments dc = (UserControlDocuments)sender;
        DataSet ds = new DataSet();
        UserControlDate issuedate;
        UserControlDate expirydate;
        if (dc.SelectedDocument != "Dummy")
        {
            ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(General.GetNullableInteger(dc.SelectedDocument).Value);
        }
        if (ds.Tables.Count > 0)
        {
            if (dc.ID == "ddlLicenceAdd")
            {
                GridFooterItem Item = (GridFooterItem)dc.NamingContainer;

                issuedate = (UserControlDate)Item.FindControl("txtIssueDateAdd");
                expirydate = (UserControlDate)Item.FindControl("txtExpiryDateAdd");
                RadTextBox txtLicenceNumberAdd = (RadTextBox)Item.FindControl("txtLicenceNumberAdd");
            }
            else
            {
                GridDataItem dataItem = (GridDataItem)dc.NamingContainer;
                issuedate = (UserControlDate)dataItem.FindControl("txtIssueDateEdit");
                expirydate = (UserControlDate)dataItem.FindControl("txtExpiryDateEdit");
                RadTextBox txtLicenceNumberEdit = (RadTextBox)dataItem.FindControl("txtLicenceNumberEdit");
                txtLicenceNumberEdit.Focus();
            }
            if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
            {
                issuedate.CssClass = "input_mandatory";
                expirydate.CssClass = "input_mandatory";
            }
            else
            {
                issuedate.CssClass = "input";
                expirydate.CssClass = "input";
            }
        }
    }

    private bool IsValidLicence(string requestdetailid, string licence, string licencenumber, string placeofissue, string dateofissue, UserControlDate dateofexpiry, string flag)
    {
        int resultInt;
        DateTime resultDate;
        DateTime dtExpiryDate = DateTime.Now;
        DateTime dtIddueDate = DateTime.Now;

        ucError.HeaderMessage = "Please provide the following required information";

        if (requestdetailid.Trim() == "")
            ucError.ErrorMessage = "Selected Licence Not Requested";
        else
        {
            if (!int.TryParse(licence, out resultInt))
                ucError.ErrorMessage = "Licence is required";

            if (licencenumber.Trim() == "")
                ucError.ErrorMessage = "Licence Number is required";

            if (placeofissue.Trim() == "")
                ucError.ErrorMessage = "Place of Issue is required";

            if (!DateTime.TryParse(dateofissue, out resultDate))
                ucError.ErrorMessage = "Issue Date is required.";
            else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Issue Date should be earlier than current date";
            }

            if (!DateTime.TryParse(dateofexpiry.Text, out resultDate) && dateofexpiry.CssClass == "input_mandatory")
                ucError.ErrorMessage = "Expiry Date is required.";

            if (!int.TryParse(flag, out resultInt))
                ucError.ErrorMessage = "Flag is required";
        }

        return (!ucError.IsError);
    }


    protected void gvSeamanBook_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindSeamansBookData();
    }

    protected void BindSeamansBookData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.SeamansMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate), General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            gvSeamanBook.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }





    protected void gvSeamanBook_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");

            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                  + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=TRAVELDOCUPLOAD'); return false;");
            }

            RadLabel lblRequestDetailId = (RadLabel)e.Item.FindControl("lblRequestDetailId");
            if (lblRequestDetailId != null)
            {
                if (lblRequestDetailId.Text == string.Empty)
                {
                    if (cme != null) cme.Visible = false;
                    if (db != null) db.Visible = false;
                    if (att != null) att.Visible = false;
                }
            }

            RadLabel lbl = (RadLabel)e.Item.FindControl("lblDocumentId");
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (img != null)
            {
                img.Attributes.Add("onclick", "openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
                if (string.IsNullOrEmpty(lblR.Text.Trim()))
                {
                    if (img != null)
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses\"></i></span>";
                        img.Controls.Add(html);
                    }

                }
            }

        }

        if (e.Item.IsInEditMode)
        {
            UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit");
            DataRowView drvDocumentType = (DataRowView)e.Item.DataItem;
            if (ucDocumentType != null) ucDocumentType.SelectedDocumentType = drvDocumentType["FLDDOCUMENTTYPE"].ToString();

            UserControlFlag ucFlag = (UserControlFlag)e.Item.FindControl("ddlFlagEdit");
            DataRowView drvFlag = (DataRowView)e.Item.DataItem;
            if (ucFlag != null) ucFlag.SelectedFlag = drvFlag["FLDFLAGID"].ToString();

        }


    }

    protected void gvSeamanBook_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvSeamanBook_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string empid = ViewState["Empid"].ToString();
            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailIdEdit")).Text;
            string documenttype = ((UserControlDocumentType)e.Item.FindControl("ucDocumentTypeEdit")).SelectedDocumentType;
            string licenceid = ((RadLabel)e.Item.FindControl("lblDocumentIdEdit")).Text;
            string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberEdit")).Text;
            string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssue")).Text;
            UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryEdit"));
            string country = ((UserControlFlag)e.Item.FindControl("ddlFlagEdit")).SelectedFlag;
            string connectedtovessel = ((CheckBox)e.Item.FindControl("chkConnectedToVesselEdit")).Checked ? "1" : "0";
            string validfrom = ((UserControlDate)e.Item.FindControl("ucValidFromEdit")).Text;
            string noofentry = ((RadComboBox)e.Item.FindControl("ddlNoofentryEdit")).SelectedValue;
            string passportno = ((RadTextBox)e.Item.FindControl("txtpassportnoEdit")).Text;

            if (!IsValidTravelDocument(requestdetailid, documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry, country))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateLicenceRequestSeamansBook(Convert.ToInt32(empid)
                                                                , new Guid(requestdetailid)
                                                                , Convert.ToInt32(documenttype)
                                                                , Convert.ToInt32(licenceid)
                                                                , documentnumber
                                                                , Convert.ToDateTime(dateofissue)
                                                                , placeofissue
                                                                , General.GetNullableDateTime(dateofexpiry.Text)
                                                                , int.Parse(country)
                                                                , int.Parse(connectedtovessel)
                                                                , General.GetNullableDateTime(validfrom)
                                                                , General.GetNullableInteger(noofentry)
                                                                , null
                                                                , General.GetNullableString(passportno)
                                                                , 1
                                                              );

            BindSeamansBookData();
            gvSeamanBook.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvSeamanBook_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailId")).Text;

            if (requestdetailid == "")
            {
                ucError.ErrorMessage = "Cannot Delete. Not a valid request document";
                ucError.Visible = true;
            }
            else
            {
                PhoenixCrewLicenceRequest.DeleteLicenceRequestDocument(new Guid(requestdetailid));
            }

            BindSeamansBookData();
            gvSeamanBook.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTravelDocument(string requestdetailid, string documenttype, string documentnumber, string dateofissue, string placeofissue, UserControlDate dateofexpiry, string country)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        DateTime resultdate;

        if (requestdetailid.Trim() == "")
            ucError.ErrorMessage = "Selected Licence Not Requested";

        if (documenttype.Equals("") || !Int16.TryParse(documenttype, out result))
            ucError.ErrorMessage = "Document Type  is required";

        if (documentnumber.Trim() == "")
            ucError.ErrorMessage = "Document Number is required";

        if (dateofissue == null)
            ucError.ErrorMessage = "Date of Issue is required";
        else if (DateTime.TryParse(dateofissue, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Date of Issue should be earlier than current date";
        }

        if (placeofissue.Trim() == "")
            ucError.ErrorMessage = "Place of Issue is required";

        if (string.IsNullOrEmpty(dateofexpiry.Text) && dateofexpiry.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Expiry Date is required";
        else if (!string.IsNullOrEmpty(dateofissue)
            && DateTime.TryParse(dateofexpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(dateofissue)) < 0)
        {
            ucError.ErrorMessage = "Date of Expiry should be later than 'Date of Issue'";
        }
        if (country.Equals("") || !Int16.TryParse(country, out result))
            ucError.ErrorMessage = "Nationality/Flag  is required";

        return (!ucError.IsError);
    }

    protected void gvCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCourseData();
    }


    protected void BindCourseData()
    {
        try
        {
            string strEmployeeId = ViewState["Empid"].ToString();
            string strRankId = ViewState["Rankid"].ToString();
            string strVesselId = ViewState["Vesselid"].ToString();
            string strCrewChangeDate = ViewState["Date"].ToString();

            DataTable dt = PhoenixCrewLicence.CourseMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strCrewChangeDate), General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            gvCourse.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            Image imgFlag = (Image)e.Item.FindControl("imgFlag");
            RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
            RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");

            if (lblMissingYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/blue.png";
            }
            else if (lblExpiredYN.Text == "1")
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/red.png";
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                   + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEUPLOAD'); return false;");
            }

            RadLabel lblRequestDetailId = (RadLabel)e.Item.FindControl("lblRequestDetailId");
            if (lblRequestDetailId != null)
            {
                if (lblRequestDetailId.Text == string.Empty)
                {
                    if (cme != null) cme.Visible = false;
                    if (db != null) db.Visible = false;
                    if (att != null) att.Visible = false;
                }
            }

            LinkButton imgRemarks = (LinkButton)e.Item.FindControl("cmdAddInstruction");
            if (imgRemarks != null)
            {
                if (drv["FLDREMARKSYN"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses\"></i></span>";
                    imgRemarks.Controls.Add(html);
                }
            }
        }

        if (e.Item.IsInEditMode)
        {
            UserControlCourse ucCourseEdit = (UserControlCourse)e.Item.FindControl("ucCourseEdit");

            DataRowView drvcourse = (DataRowView)e.Item.DataItem;
            if (ucCourseEdit != null)
            {
                ucCourseEdit.CourseList = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
                ucCourseEdit.bind();
                ucCourseEdit.SelectedCourse = drvcourse["FLDDOCUMENTID"].ToString();
            }

            UserControlFlag ucFlag = (UserControlFlag)e.Item.FindControl("ddlFlagEdit");
            DataRowView drvFlag = (DataRowView)e.Item.DataItem;
            if (ucFlag != null) ucFlag.SelectedFlag = drvFlag["FLDFLAGID"].ToString();

            UserControlAddressType ucInstitutionEdit = (UserControlAddressType)e.Item.FindControl("ucInstitutionEdit");
            DataRowView drvInstitute = (DataRowView)e.Item.DataItem;
            if (ucInstitutionEdit != null) ucInstitutionEdit.SelectedAddress = drvInstitute["FLDINSTITUTIONID"].ToString();

        }

    }

    protected void gvCourse_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "INSTRUCTION")
            {
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Common/CommonDiscussion.aspx?');");

                string lblDTKey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;

                PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
                objdiscussion.dtkey = new Guid(lblDTKey);
                objdiscussion.userid = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                objdiscussion.title = PhoenixCrewConstants.REMARKSTITLE;
                objdiscussion.type = PhoenixCrewConstants.REMARKS;
                PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCourse_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string empid = ViewState["Empid"].ToString();
            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailIdEdit")).Text;
            string institutionid = ((UserControlAddressType)e.Item.FindControl("ucInstitutionEdit")).SelectedAddress;
            string courseid = ((RadLabel)e.Item.FindControl("lblDocumentIdEdit")).Text;
            string documentnumber = ((RadTextBox)e.Item.FindControl("txtNumberEdit")).Text;
            string dateofissue = ((UserControlDate)e.Item.FindControl("ucDateOfIssueEdit")).Text;
            string placeofissue = ((RadTextBox)e.Item.FindControl("txtPlaceOfIssue")).Text;
            UserControlDate dateofexpiry = ((UserControlDate)e.Item.FindControl("ucDateExpiryEdit"));
            string country = ((UserControlFlag)e.Item.FindControl("ddlFlagEdit")).SelectedFlag;
            string authority = ((RadTextBox)e.Item.FindControl("txtAuthorityEdit")).Text;

            if (!IsValidCourseCertificate(requestdetailid, courseid, documentnumber, dateofissue, dateofexpiry, placeofissue, institutionid, country))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateLicenceRequestCourse(Convert.ToInt32(empid)
                                                                , new Guid(requestdetailid)
                                                                , Convert.ToInt32(courseid)
                                                                , documentnumber
                                                                , Convert.ToDateTime(dateofissue)
                                                                , placeofissue
                                                                , General.GetNullableDateTime(dateofexpiry.Text)
                                                                , int.Parse(institutionid)
                                                                , int.Parse(country)
                                                                , authority
                                                                , byte.Parse("0")
                                                             );
            BindCourseData();
            gvCourse.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCourse_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string requestdetailid = ((RadLabel)e.Item.FindControl("lblRequestDetailId")).Text;

            if (requestdetailid == "")
            {
                ucError.ErrorMessage = "Cannot Delete. Not a valid request document";
                ucError.Visible = true;
            }
            else
            {
                PhoenixCrewLicenceRequest.DeleteLicenceRequestDocument(new Guid(requestdetailid));
            }

            BindCourseData();
            gvCourse.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCourseCertificate(string requestdetailid, string courseid, string certificatenumber, string dateofissue, UserControlDate dateofexpiry, string placeofissue, string institutionid, string flagid)
    {
        Int16 resultInt;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (requestdetailid.Trim() == "")
            ucError.ErrorMessage = "Selected Licence Not Requested";
        else
        {
            if (!Int16.TryParse(courseid, out resultInt))
                ucError.ErrorMessage = "Course is required";


            if (!Int16.TryParse(institutionid, out resultInt))
                ucError.ErrorMessage = "Institution is required";

            if (!Int16.TryParse(flagid, out resultInt))
                ucError.ErrorMessage = "Flag is required";

            if (certificatenumber.Trim() == "")
                ucError.ErrorMessage = "Certificate Number is required";

            if (placeofissue.Trim() == "")
                ucError.ErrorMessage = "Place of issue is required";

            if (string.IsNullOrEmpty(dateofissue))
                ucError.ErrorMessage = "Issue Date is required.";
            else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Issue Date should be earlier than current date";
            }

            if (string.IsNullOrEmpty(dateofexpiry.Text) && dateofexpiry.CssClass == "input_mandatory")
                ucError.ErrorMessage = "Expiry Date is required.";

            if (dateofissue != null && dateofexpiry.Text != null)
            {
                if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry.Text, out resultDate)))
                    if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry.Text)))
                        ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
            }
        }

        return (!ucError.IsError);
    }


}