using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Crew_CrewMissingLicenceRequest : PhoenixBasePage
{
    string strEmployeeId = string.Empty, strRankId = string.Empty, strVesselId = string.Empty;
    string strJoinDate = string.Empty; string strFromPage = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            strEmployeeId = Request.QueryString["empid"];
            strRankId = Request.QueryString["rnkid"];
            strVesselId = Request.QueryString["vslid"];
            strJoinDate = Request.QueryString["jdate"];

            if (Request.QueryString["from"] != null)
            {
                strFromPage = Request.QueryString["from"];
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Request Licence", "REQUEST", ToolBarDirection.Right);
            toolbar.AddButton("Refresh", "REFRESH", ToolBarDirection.Right);
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");

                SetInformation();

                ddlFlagAddress.DataSource = PhoenixRegistersAddress.ListFlagAddress(General.GetNullableInteger(ViewState["flag"].ToString()));
                ddlFlagAddress.DataTextField = "FLDNAME";
                ddlFlagAddress.DataValueField = "FLDADDRESSCODE";
                ddlFlagAddress.DataBind();

                SetEmployeePrimaryDetails();

                ViewState["PAGENUMBER"] = 1;
            }
            gvMissingLicence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("REQUEST"))
            {
                string csvLicence = string.Empty, csvType = string.Empty;
                foreach (GridDataItem dataItem in gvFlag.MasterTableView.Items)
                {

                    RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                    if (ck != null)
                    {
                        if (ck.Checked == true && ck.Enabled == true)
                        {
                            csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                            csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                        }
                    }
                }

                foreach (GridDataItem dataItem in gvDCE.MasterTableView.Items)
                {

                    RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                    if (ck != null)
                    {
                        if (ck.Checked == true && ck.Enabled == true)
                        {
                            csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                            csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                        }
                    }
                }

                foreach (GridDataItem dataItem in gvSeamanBook.MasterTableView.Items)
                {

                    RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                    if (ck != null)
                    {
                        if (ck.Checked == true && ck.Enabled == true)
                        {
                            csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                            csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                        }
                    }
                }

                foreach (GridDataItem dataItem in gvCourse.MasterTableView.Items)
                {

                    RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                    if (ck != null)
                    {
                        if (ck.Checked == true && ck.Enabled == true)
                        {
                            csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                            csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                        }
                    }
                }


                if (!IsValidateRequest(csvLicence.TrimEnd(','), txtCrewChangeDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                RadWindowManager1.RadConfirm("Are you sure you want to Initiate Licence Request?", "confirm", 320, 125, null, "Confirm");
            }
            if (CommandName.ToUpper().Equals("REFRESH"))
            {

                strJoinDate = txtCrewChangeDate.Text;

                if (string.IsNullOrEmpty(txtCrewChangeDate.Text))
                {
                    ucError.ErrorMessage = "Enter Correct Date format";
                    ucError.Visible = true;
                    return;
                }

                BindData();
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

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            string csvLicence = string.Empty, csvType = string.Empty, csvMissingyn = string.Empty;
            foreach (GridDataItem dataItem in gvFlag.MasterTableView.Items)
            {
                // TODO: Cast (GridDataItem)dataItem here and remove foreach 
                RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                if (ck != null)
                {
                    if (ck.Checked == true && ck.Enabled == true)
                    {
                        csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                        csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                        csvMissingyn += ((RadLabel)dataItem.FindControl("lblMissingYN")).Text + ",";
                    }
                }
            }

            foreach (GridDataItem dataItem in gvDCE.MasterTableView.Items)
            {
                // TODO: Cast (GridDataItem)dataItem here and remove foreach 
                RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                if (ck != null)
                {
                    if (ck.Checked == true && ck.Enabled == true)
                    {
                        csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                        csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                        csvMissingyn += ((RadLabel)dataItem.FindControl("lblMissingYN")).Text + ",";
                    }
                }
            }

            foreach (GridDataItem dataItem in gvSeamanBook.MasterTableView.Items)
            {
                // TODO: Cast (GridDataItem)dataItem here and remove foreach 
                RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                if (ck != null)
                {
                    if (ck.Checked == true && ck.Enabled == true)
                    {
                        csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                        csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                        csvMissingyn += ((RadLabel)dataItem.FindControl("lblMissingYN")).Text + ",";
                    }
                }
            }

            foreach (GridDataItem dataItem in gvCourse.MasterTableView.Items)
            {
                // TODO: Cast (GridDataItem)dataItem here and remove foreach 
                RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                if (ck != null)
                {
                    if (ck.Checked == true && ck.Enabled == true)
                    {
                        csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                        csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                        csvMissingyn += ((RadLabel)dataItem.FindControl("lblMissingYN")).Text + ",";
                    }
                }
            }

            Guid? requestid = null;

            PhoenixCrewLicenceRequest.InsertLicenceRequest(int.Parse(strEmployeeId),
                                                            General.GetNullableInteger(ViewState["flag"].ToString()),
                                                            General.GetNullableInteger(strVesselId),
                                                            General.GetNullableInteger(strRankId),
                                                            General.GetNullableDateTime(txtCrewChangeDate.Text),
                                                            csvLicence.TrimEnd(','),
                                                            csvType.TrimEnd(','),
                                                            General.GetNullableInteger(ddlFlagAddress.SelectedValue)
                                                            , ref requestid
                                                            , csvMissingyn.TrimEnd(',')
                                                            , General.GetNullableDateTime(txtReqDate.Text));

            if (strFromPage == "LicenceAdd")
            {
                Response.Redirect("../Crew/CrewLicenceRequestDetailList.aspx?rid=" + requestid, false);
            }
            ucStatus.Text = "Licence Request Initiated";
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
    private void SetInformation()
    {
        DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(strVesselId));
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            txtVesselFlag.Text = ds.Tables[0].Rows[0]["FLDFLAGNAME"].ToString();
            ViewState["flag"] = ds.Tables[0].Rows[0]["FLDFLAG"].ToString();
            txtCrewChangeDate.Text = strJoinDate;
            txtReqDate.Text = DateTime.Now.Date.ToString();
        }
    }

    protected void ddlFlagAddress_DataBound(object sender, EventArgs e)
    {
        ddlFlagAddress.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(strEmployeeId));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
                txtFileNo.Text = dt.Rows[0]["FLDFILENO"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidateRequest(string csvLicence, string date)
    {
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(csvLicence))
            ucError.ErrorMessage = "select atleast one or more licence.";

        if (string.IsNullOrEmpty(txtCrewChangeDate.Text) && txtCrewChangeDate.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Date is required.";

        else if (DateTime.TryParse(txtCrewChangeDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "Date should be greater than current date";
        }

        return (!ucError.IsError);
    }

    protected void BindData()
    {
        try
        {
            DataTable dt = PhoenixCrewLicence.NationalLicenceMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), General.GetNullableDateTime(strJoinDate));
            if (dt.Rows.Count > 0)
            {
                gvMissingLicence.DataSource = dt;
                //gvCrewLicence.VirtualItemCount = iRowCount;
            }
            else
            {
                gvMissingLicence.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindFlagData()
    {
        try
        {
            DataTable dt = PhoenixCrewLicence.FlagLicenceMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), General.GetNullableDateTime(strJoinDate), null);
            if (dt.Rows.Count > 0)
            {
                gvFlag.DataSource = dt;
            }
            else
            {
                gvFlag.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindDCEData()
    {
        try
        {
            DataTable dt = PhoenixCrewLicence.DCEMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), General.GetNullableDateTime(strJoinDate), null);
            if (dt.Rows.Count > 0)
            {
                gvDCE.DataSource = dt;
            }
            else
            {
                gvDCE.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindSeamansBookData()
    {
        try
        {
            DataTable dt = PhoenixCrewLicence.SeamansMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), General.GetNullableDateTime(strJoinDate), null);
            if (dt.Rows.Count > 0)
            {
                gvSeamanBook.DataSource = dt;
            }
            else
            {
                gvSeamanBook.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void InitiateLicenceRequest(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                string csvLicence = string.Empty, csvType = string.Empty, csvMissingyn = string.Empty;
                foreach (GridDataItem dataItem in gvFlag.MasterTableView.Items)
                {
                    RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                    if (ck != null)
                    {
                        if (ck.Checked == true && ck.Enabled == true)
                        {
                            csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                            csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                            csvMissingyn += ((RadLabel)dataItem.FindControl("lblMissingYN")).Text + ",";
                        }
                    }
                }

                foreach (GridDataItem dataItem in gvDCE.MasterTableView.Items)
                {
                    RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                    if (ck != null)
                    {
                        if (ck.Checked == true && ck.Enabled == true)
                        {
                            csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                            csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                            csvMissingyn += ((RadLabel)dataItem.FindControl("lblMissingYN")).Text + ",";
                        }
                    }
                }

                foreach (GridDataItem dataItem in gvSeamanBook.MasterTableView.Items)
                {
                    RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                    if (ck != null)
                    {
                        if (ck.Checked == true && ck.Enabled == true)
                        {
                            csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                            csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                            csvMissingyn += ((RadLabel)dataItem.FindControl("lblMissingYN")).Text + ",";
                        }
                    }
                }
                foreach (GridDataItem dataItem in gvCourse.MasterTableView.Items)
                {
                    // TODO: Cast (GridDataItem)dataItem here and remove foreach 
                    RadCheckBox ck = (RadCheckBox)dataItem.FindControl("chkSelect");
                    if (ck != null)
                    {
                        if (ck.Checked == true && ck.Enabled == true)
                        {
                            csvLicence += ((RadLabel)dataItem.FindControl("lblDocumentId")).Text + ",";
                            csvType += ((RadLabel)dataItem.FindControl("lblType")).Text + ",";
                            csvMissingyn += ((RadLabel)dataItem.FindControl("lblMissingYN")).Text + ",";
                        }
                    }
                }

                Guid? requestid = null;

                PhoenixCrewLicenceRequest.InsertLicenceRequest(int.Parse(strEmployeeId),
                                                                General.GetNullableInteger(ViewState["flag"].ToString()),
                                                                General.GetNullableInteger(strVesselId),
                                                                General.GetNullableInteger(strRankId),
                                                                General.GetNullableDateTime(txtCrewChangeDate.Text),
                                                                csvLicence.TrimEnd(','),
                                                                csvType.TrimEnd(','),
                                                                General.GetNullableInteger(ddlFlagAddress.SelectedValue)
                                                                , ref requestid
                                                                , csvMissingyn.TrimEnd(',')
                                                                , General.GetNullableDateTime(txtReqDate.Text));

                if (strFromPage == "LicenceAdd")
                {
                    Response.Redirect("../Crew/CrewLicenceRequestDetailList.aspx?rid=" + requestid, false);
                }
                ucStatus.Text = "Licence Request Initiated";
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

    protected void BindCourseData()
    {
        try
        {
            DataTable dt = PhoenixCrewLicence.CourseMissingList(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), General.GetNullableDateTime(strJoinDate), null);
            if (dt.Rows.Count > 0)
            {
                gvCourse.DataSource = dt;
            }
            else
            {
                gvCourse.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMissingLicence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMissingLicence.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMissingLicence_ItemDataBound(object sender, GridItemEventArgs e)
    {
        Image imgFlag = (Image)e.Item.FindControl("imgFlag");
        RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
        RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
        if (imgFlag != null)
        {
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

        RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
        RadLabel lbl = (RadLabel)e.Item.FindControl("lblLicenceId");
        LinkButton imgRemarks = (LinkButton)e.Item.FindControl("imgRemarks");
        RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
        if (imgRemarks != null)
        {
            if (string.IsNullOrEmpty(lblR.Text.Trim()))
            {
                HtmlGenericControl html = new HtmlGenericControl();                
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                imgRemarks.Controls.Add(html);
            }

            if (string.IsNullOrEmpty(lbl.Text.Trim()))
            {
                imgRemarks.Enabled = false;
            }
            else
            {
                imgRemarks.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.LICENCE + "','xlarge')");                
            }

        }
        RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
        if (lbtn != null)
        {
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
    }


    protected void gvFlag_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        RadCheckBox ck1 = e.Item.FindControl("chkSelect") as RadCheckBox;
        if (ck1 != null)
        {
            if (drv["FLDISREQ"].ToString() == "1") ck1.Enabled = false;
        }
        Image imgFlag = (Image)e.Item.FindControl("imgFlag");
        RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
        RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
        if (imgFlag != null)
        {
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

    protected void gvFlag_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFlag.CurrentPageIndex + 1;
            BindFlagData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDCE_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        RadCheckBox ck1 = e.Item.FindControl("chkSelect") as RadCheckBox;
        if (ck1 != null)
        {
            if (drv["FLDISREQ"].ToString() == "1") ck1.Enabled = false;
        }

        Image imgFlag = (Image)e.Item.FindControl("imgFlag");
        RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
        RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
        if (imgFlag != null)
        {
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

        RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLicense");
        if (lbtn != null)
        {
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
        RadLabel lbtn1 = (RadLabel)e.Item.FindControl("lblDCEConnectedToVessel");
        if (lbtn1 != null)
        {
            UserControlToolTip uct1 = (UserControlToolTip)e.Item.FindControl("ucToolTipDCEConnectedToVessel");
            lbtn1.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct1.ToolTip + "', 'visible');");
            lbtn1.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct1.ToolTip + "', 'hidden');");
        }

    }

    protected void gvDCE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDCE.CurrentPageIndex + 1;
            BindDCEData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSeamanBook_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSeamanBook.CurrentPageIndex + 1;
            BindSeamansBookData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSeamanBook_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        RadCheckBox ck = e.Item.FindControl("chkSelect") as RadCheckBox;
        if (ck != null)
        {
            if (drv["FLDISREQ"].ToString() == "1") ck.Enabled = false;
        }

        Image imgFlag = (Image)e.Item.FindControl("imgFlag");
        RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
        RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
        if (imgFlag != null)
        {
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

        RadLabel lbl = (RadLabel)e.Item.FindControl("lblDocumentId");
        LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
        RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
        if (img != null)
        {
            if (string.IsNullOrEmpty(lblR.Text.Trim()))
            {
                HtmlGenericControl html = new HtmlGenericControl();                
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                img.Controls.Add(html);
            }
            if (string.IsNullOrEmpty(lbl.Text.Trim()))
            {
                img.Enabled = false;
            }
            else
            {
                img.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
            }
        }
    }

    protected void gvCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCourse.CurrentPageIndex + 1;
            BindCourseData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        RadCheckBox ck = e.Item.FindControl("chkSelect") as RadCheckBox;
        if (ck != null)
        {
            if (drv["FLDISREQ"].ToString() == "1") ck.Enabled = false;
        }
        Image imgFlag = (Image)e.Item.FindControl("imgFlag");
        RadLabel lblMissingYN = (RadLabel)e.Item.FindControl("lblMissingYN");
        RadLabel lblExpiredYN = (RadLabel)e.Item.FindControl("lblExpiredYN");
        if (imgFlag != null)
        {
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
        RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVerified");
        UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipCertificates");
        if (lbtn != null)
        {
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }

        RadLabel lbl = (RadLabel)e.Item.FindControl("lblDocumentId");
        LinkButton img = (LinkButton)e.Item.FindControl("cmdAddInstruction");
        RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
        if (img != null)
        {
            if (drv["FLDREMARKSYN"].ToString() == "0")
            {
                HtmlGenericControl html = new HtmlGenericControl();                
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                img.Controls.Add(html);
            }

        }
    }
}

