using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class Registers_RegisterVisa : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            if (!Page.IsPostBack)
            {
                PhoenixToolbar toolbargrid = new PhoenixToolbar();

                if (Request.QueryString["countryid"] != null)
                {
                    ViewState["countryid"] = Request.QueryString["countryid"].ToString();
                    ViewState["seafarerVisa"] = PhoenixCommonRegisters.GetHardCode(1, 107, "OFF");
                    ViewState["familyVisa"] = PhoenixCommonRegisters.GetHardCode(1, 107, "FMY");
                }

                ViewState["PAGENUMBER"] = 1;
                BindInformation();
                gvVisa.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            // BindData();
            //BindFamily();

            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVisaList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindInformation()
    {
        DataSet ds = PhoenixRegistersCountry.EditCountry(int.Parse(ViewState["countryid"].ToString()));

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    Title.Text = "Visa List" + " (" + ds.Tables[0].Rows[0]["FLDCOUNTRYNAME"].ToString() + ")";
        //}
    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOUNTRYNAME", "FLDVISATYPENAME", "FLDTIMETAKEN", "FLDDAYSREQUIREDFORVISA", "FLDPHYSICALPRESENCEYESNO", "FLDPHYSICALPRESENCESPECIFICATION", "FLDURGENTPROCEDURE", "FLDORDINARYAMOUNT", "FLDURGENTAMOUNT", "FLDREMARKS", "FLDMODIFIEDBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Country Name", "Visa Type", "Time Taken", "Days Required", "Physical Presence Y/N", "Physical Presence Specification", "Urgent Procedure", "Ordinary Amount(USD)", "Urgent Amount(USD)", "Remarks", "Last Modified By", "Modified Date" };

        DataSet ds = PhoenixRegistersCountryVisa.VisaSearch(General.GetNullableInteger(ViewState["countryid"].ToString())
                                                            , (int)ViewState["PAGENUMBER"]
                                                            , gvVisa.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);

        General.SetPrintOptions("gvVisa", "Visa", alCaptions, alColumns, ds);
        gvVisa.DataSource = ds;
        gvVisa.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }




    private void UpdateCountryVisa(string visaid, string countrycode, string visatypeid, string locSubmission, string daysrequired, int physicalpresence
                                    , string physicalpresencespecification, string urgentprocedure, string remarks, string ordinaryamount, string urgentamount, int notvalidonoldpp
                                    , int Onarrival)
    {
        if (!IsValidCountryVisa(countrycode, visatypeid,
            locSubmission, daysrequired,
            physicalpresence, physicalpresencespecification, urgentprocedure))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersCountryVisa.UpdateCountryVisa(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(visaid),
            int.Parse(countrycode),
            int.Parse(visatypeid),
            locSubmission, daysrequired, physicalpresence,
            physicalpresencespecification, urgentprocedure, remarks,
            General.GetNullableDecimal(ordinaryamount), General.GetNullableDecimal(urgentamount), notvalidonoldpp, Onarrival);
    }

    private void DeleteCountryVisa(string visaid)
    {
        PhoenixRegistersCountryVisa.DeleteCountryVisa(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(visaid));
    }

    private void InsertCountryVisa(string countrycode, string visatype, string locSubmission, string daysrequired
                                   , int physicalpresence, string physicalpresencespecification, string urgentprocedure, string remarks,
                                    string ordinaryamount, string urgentamount, int notvalidonoldpp, int Onarrival)
    {
        if (!IsValidCountryVisa(countrycode, visatype,
            locSubmission, daysrequired,
            physicalpresence, physicalpresencespecification, urgentprocedure))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersCountryVisa.InsertCountryVisa(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                      int.Parse(countrycode),
                                                      int.Parse(visatype),
                                                      locSubmission, daysrequired, physicalpresence, physicalpresencespecification, urgentprocedure, remarks,
                                                      General.GetNullableDecimal(ordinaryamount), General.GetNullableDecimal(urgentamount), notvalidonoldpp
                                                      , Onarrival);

    }
    private bool IsValidCountryVisa(string countryname, string visatype, string timetaken, string daysrequired
                                        , int physicalpresence, string physicalpresencespecification, string urgentprocedure)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(countryname) == null)
            ucError.ErrorMessage = "Country Name is required.";

        if (General.GetNullableInteger(visatype) == null)
            ucError.ErrorMessage = "Visa type is required.";

        if (timetaken.Trim().Equals(""))
            ucError.ErrorMessage = "Time taken is required.";

        if (daysrequired.Trim().Equals(""))
            ucError.ErrorMessage = "Days required is required.";

        if (physicalpresence == 1)
            if (physicalpresencespecification == string.Empty)
                ucError.ErrorMessage = "Physical presence specification is required";

        if (urgentprocedure.Trim().Equals(""))
            ucError.ErrorMessage = "Urgent procedure is required.";

        return (!ucError.IsError);
    }



    private void BindFamily()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOUNTRYNAME", "FLDVISATYPENAME", "FLDTIMETAKEN", "FLDDAYSREQUIREDFORVISA", "FLDPHYSICALPRESENCEYESNO", "FLDPHYSICALPRESENCESPECIFICATION", "FLDURGENTPROCEDURE", "FLDORDINARYAMOUNT", "FLDURGENTAMOUNT", "FLDREMARKS", "FLDMODIFIEDBYNAME", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Country Name", "Visa Type", "Time Taken", "Days Required", "Physical Presence Y/N", "Physical Presence Specification", "Urgent Procedure", "Ordinary Amount(USD)", "Urgent Amount(USD)", "Remarks", "Last Modified By", "Modified Date" };

        DataSet df = PhoenixRegistersCountryVisa.FamilyVisaSearch(General.GetNullableInteger(ViewState["countryid"].ToString())
                                                           , (int)ViewState["PAGENUMBER"]
                                                           , gvFVisa.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount);

        General.SetPrintOptions("gvFVisa", "Family Visa", alCaptions, alColumns, df);
        gvFVisa.DataSource = df;
        gvFVisa.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }



    protected void gvFVisa_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindFamily();
    }


    protected void gvFVisa_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindFamily();

    }

    protected void gvFVisa_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindFamily();
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
        BindData();
        BindFamily();
    }



    protected void gvVisa_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVisa.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVisa_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if ((((CheckBox)e.Item.FindControl("chkPhysicalPresenceYNAdd")).Checked) == false)
                    ((RadTextBox)e.Item.FindControl("txtPhysicalPresenceSpecificationAdd")).Text = "";

                RadLabel Remarks = ((RadLabel)e.Item.FindControl("txtRemarksAdd"));
                string Remarkstext = Remarks.Text;

                string countrid = ViewState["countryid"].ToString();

                InsertCountryVisa(countrid,
                                   ViewState["seafarerVisa"].ToString(),
                                  ((RadTextBox)e.Item.FindControl("txtLocSubmissionAdd")).Text,
                                  ((RadTextBox)e.Item.FindControl("txtDaysRequiredAdd")).Text,
                                  (((CheckBox)e.Item.FindControl("chkPhysicalPresenceYNAdd")).Checked) ? 1 : 0,
                                  ((RadTextBox)e.Item.FindControl("txtPhysicalPresenceSpecificationAdd")).Text,
                                  ((RadTextBox)e.Item.FindControl("txtUrgentProcedureAdd")).Text,
                                  Remarkstext,
                                  ((UserControlMaskNumber)e.Item.FindControl("txtOrdinaryAmountAdd")).Text,
                                  ((UserControlMaskNumber)e.Item.FindControl("txtUrgentAmonutAdd")).Text,
                                  (((CheckBox)e.Item.FindControl("chkPassportYNAdd")).Checked) ? 1 : 0,
                                  (((CheckBox)e.Item.FindControl("chkOnArrivalAdd")).Checked) ? 1 : 0
                                );

                BindData();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteCountryVisa(((RadLabel)e.Item.FindControl("lblVisaID")).Text);
                BindData();
            }
            if (e.CommandName.ToUpper() == "UPDATE")
            {

                if ((((CheckBox)e.Item.FindControl("chkEditPhysicalPresenceYN")).Checked) == false)
                    ((RadTextBox)e.Item.FindControl("txtPhysicalPresenceSpecificationEdit")).Text = "";

                UpdateCountryVisa(
                      ((RadLabel)e.Item.FindControl("lblEditVisaID")).Text,
                       ((RadLabel)e.Item.FindControl("lblEditCountryID")).Text,
                       ((RadLabel)e.Item.FindControl("lblEditVisaTypeID")).Text,
                       ((RadTextBox)e.Item.FindControl("txtEditLocSubmission")).Text,
                       ((RadTextBox)e.Item.FindControl("txtEditDaysRequired")).Text,
                       (((CheckBox)e.Item.FindControl("chkEditPhysicalPresenceYN")).Checked) ? 1 : 0,
                       ((RadTextBox)e.Item.FindControl("txtPhysicalPresenceSpecificationEdit")).Text,
                       ((RadTextBox)e.Item.FindControl("txtUrgentProcedureEdit")).Text,
                       ((RadLabel)e.Item.FindControl("txtRemarksEdit")).Text,
                       ((UserControlMaskNumber)e.Item.FindControl("txtOrdinaryAmountEdit")).Text,
                       ((UserControlMaskNumber)e.Item.FindControl("txtUrgentAmonutEdit")).Text,
                       (((CheckBox)e.Item.FindControl("chkPassportYNEdit")).Checked) ? 1 : 0,
                       (((CheckBox)e.Item.FindControl("chkEditOnArrival")).Checked) ? 1 : 0);


                BindData();
                gvVisa.Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            //    SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvFVisa_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFVisa.CurrentPageIndex + 1;
            BindFamily();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFVisa_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            if ((((CheckBox)e.Item.FindControl("chkPhysicalPresenceYNAdd")).Checked) == false)
                ((RadTextBox)e.Item.FindControl("txtPhysicalPresenceSpecificationAdd")).Text = "";

            RadLabel Remarks = ((RadLabel)e.Item.FindControl("txtRemarksAdd"));
            string Remarkstext = Remarks.Text;

            string countrid = ViewState["countryid"].ToString();

            InsertCountryVisa(countrid,
                               ViewState["familyVisa"].ToString(),
                              ((RadTextBox)e.Item.FindControl("txtLocSubmissionAdd")).Text,
                              ((RadTextBox)e.Item.FindControl("txtDaysRequiredAdd")).Text,
                              (((CheckBox)e.Item.FindControl("chkPhysicalPresenceYNAdd")).Checked) ? 1 : 0,
                              ((RadTextBox)e.Item.FindControl("txtPhysicalPresenceSpecificationAdd")).Text,
                              ((RadTextBox)e.Item.FindControl("txtUrgentProcedureAdd")).Text,
                              Remarkstext,
                              ((UserControlMaskNumber)e.Item.FindControl("txtOrdinaryAmountAdd")).Text,
                              ((UserControlMaskNumber)e.Item.FindControl("txtUrgentAmonutAdd")).Text,
                              (((CheckBox)e.Item.FindControl("chkPassportYNAdd")).Checked) ? 1 : 0,
                              (((CheckBox)e.Item.FindControl("chkOnArrivalAdd")).Checked) ? 1 : 0
                            );

            BindFamily();
        }
        if (e.CommandName.ToUpper().Equals("DELETEFAMILY"))
        {
            DeleteCountryVisa(((RadLabel)e.Item.FindControl("lblFVisaId")).Text);
            BindFamily();
        }
        if (e.CommandName.ToUpper() == "UPDATE")
        {

            if ((((CheckBox)e.Item.FindControl("chkEditPhysicalPresenceYN")).Checked) == false)
                ((RadTextBox)e.Item.FindControl("txtPhysicalPresenceSpecificationEdit")).Text = "";

            UpdateCountryVisa(
                  ((RadLabel)e.Item.FindControl("lblEditVisaID")).Text,
                   ((RadLabel)e.Item.FindControl("lblEditCountryID")).Text,
                   ((RadLabel)e.Item.FindControl("lblEditVisaTypeID")).Text,
                   ((RadTextBox)e.Item.FindControl("txtEditLocSubmission")).Text,
                   ((RadTextBox)e.Item.FindControl("txtEditDaysRequired")).Text,
                   (((CheckBox)e.Item.FindControl("chkEditPhysicalPresenceYN")).Checked) ? 1 : 0,
                   ((RadTextBox)e.Item.FindControl("txtPhysicalPresenceSpecificationEdit")).Text,
                   ((RadTextBox)e.Item.FindControl("txtUrgentProcedureEdit")).Text,
                   ((RadLabel)e.Item.FindControl("txtRemarksEdit")).Text,
                   ((UserControlMaskNumber)e.Item.FindControl("txtOrdinaryAmountEdit")).Text,
                   ((UserControlMaskNumber)e.Item.FindControl("txtUrgentAmonutEdit")).Text,
                   (((CheckBox)e.Item.FindControl("chkPassportYNEdit")).Checked) ? 1 : 0,
                   (((CheckBox)e.Item.FindControl("chkEditOnArrival")).Checked) ? 1 : 0);


            BindFamily();
            gvFVisa.Rebind();
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvFVisa_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadLabel lblVisaId = (RadLabel)e.Item.FindControl("lblFVisaId");
            RadLabel lblCountry = (RadLabel)e.Item.FindControl("lblFCountry");

            LinkButton cmdVisaDocument = (LinkButton)e.Item.FindControl("cmdVisaDocuments");

            if (cmdVisaDocument != null)
            {
                cmdVisaDocument.Visible = SessionUtil.CanAccess(this.ViewState, cmdVisaDocument.CommandName);
                cmdVisaDocument.Attributes.Add("onclick", "openNewWindow('AddVisaDocument'" + ",'Country Visa Document'" + ", '"+Session["sitepath"]+"/Registers/RegistersCountryVisaDocument.aspx?visaid=" + lblVisaId.Text + "&countryname=" + lblCountry.Text + "'); return false;");
            }

            LinkButton cmdSendMail = (LinkButton)e.Item.FindControl("cmdSendMail");

            if (cmdSendMail != null)
            {
                cmdSendMail.Visible = SessionUtil.CanAccess(this.ViewState, cmdSendMail.CommandName);
                cmdSendMail.Attributes.Add("onclick", "openNewWindow('codehelp2','','" + Session["sitepath"] + "/Registers/RegistersCountryVisaEmail.aspx?visaid=" + lblVisaId.Text + "'); return false;");
            }

            LinkButton cmdExcel = (LinkButton)e.Item.FindControl("cmdExcel");

            if (cmdExcel != null)
            {
                cmdExcel.Visible = SessionUtil.CanAccess(this.ViewState, cmdExcel.CommandName);
                cmdExcel.Attributes.Add("onclick", "openNewWindow('codehelp3','','" + Session["sitepath"] + "/Registers/RegistersExport2XL.aspx?visaid=" + lblVisaId.Text + "'); return false;");
            }

            LinkButton  img = (LinkButton )e.Item.FindControl("imgFRemarks");

            if (img != null)
            {
                img.Attributes.Add("onclick", "javascript:openNewWindow('codehelp5','', '" + Session["sitepath"] + "/Registers/RegistersCountryVisaRemarks.aspx?id=" + lblVisaId.Text + "', 'xlarge')");
                img.Visible = General.GetNullableInteger(drv["FLDFAMILYREMARKSCOUNT"].ToString()) == 0 ? false : true;
            }
            LinkButton  imgNoRemarks = (LinkButton )e.Item.FindControl("imgFNoRemarks");

            if (imgNoRemarks != null)
            {
                imgNoRemarks.Attributes.Add("onclick", "javascript:openNewWindow('codehelp6','', '" + Session["sitepath"] + "/Registers/RegistersCountryVisaRemarks.aspx?id=" + lblVisaId.Text + "', 'xlarge')");
                imgNoRemarks.Visible = General.GetNullableInteger(drv["FLDFAMILYREMARKSCOUNT"].ToString()) == 0 ? true : false;
            }

            RadLabel lblPhysicalPresenceSpecification = (RadLabel)e.Item.FindControl("lblFPhysicalPresenceSpecification");
            UserControlToolTip ucPhyPresenceTT = (UserControlToolTip)e.Item.FindControl("ucFPhyPresenceTT");
            if (lblPhysicalPresenceSpecification != null)
            {
                ucPhyPresenceTT.Position = ToolTipPosition.TopCenter;
                ucPhyPresenceTT.TargetControlId = lblPhysicalPresenceSpecification.ClientID;
                //lblPhysicalPresenceSpecification.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPhyPresenceTT.ToolTip + "', 'visible');");
                //lblPhysicalPresenceSpecification.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPhyPresenceTT.ToolTip + "', 'hidden');");
            }

            RadLabel lblUrgentProcedure = (RadLabel)e.Item.FindControl("lblFUrgentProcedure");
            UserControlToolTip ucUrgentProcTT = (UserControlToolTip)e.Item.FindControl("ucFUrgentProcTT");
            if (lblUrgentProcedure != null)
            {
                ucUrgentProcTT.Position = ToolTipPosition.TopCenter;
                ucUrgentProcTT.TargetControlId = lblUrgentProcedure.ClientID;
                //lblUrgentProcedure.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucUrgentProcTT.ToolTip + "', 'visible');");
                //lblUrgentProcedure.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucUrgentProcTT.ToolTip + "', 'hidden');");
            }


            Guid? lblDtkey = General.GetNullableGuid(drv["FLDFAMILYDTKEY"].ToString());
            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp4','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.REGISTERS + "');return true;");
                cmdAttachment.Visible = General.GetNullableInteger(drv["FLDFAMILYATTACHMENTCOUNT"].ToString()) == 0 ? false : true;
            }
            LinkButton cmdNoAttachment = (LinkButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp5','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.REGISTERS + "');return true;");
                cmdNoAttachment.Visible = General.GetNullableInteger(drv["FLDFAMILYATTACHMENTCOUNT"].ToString()) == 0 ? true : false;
            }

        }



    }

    protected void gvVisa_ItemDataBound1(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadLabel lblVisaId = (RadLabel)e.Item.FindControl("lblVisaID");
            RadLabel lblCountry = (RadLabel)e.Item.FindControl("lblCountryName");

            LinkButton cmdVisaDocument = (LinkButton)e.Item.FindControl("cmdVisaDocuments");

            if (cmdVisaDocument != null)
            {
                cmdVisaDocument.Visible = SessionUtil.CanAccess(this.ViewState, cmdVisaDocument.CommandName);
                cmdVisaDocument.Attributes.Add("onclick", "openNewWindow('AddVisaDocument'" + ",'Country Visa Document'" + ", '" + Session["sitepath"] + "/Registers/RegistersCountryVisaDocument.aspx?visaid=" + lblVisaId.Text + "&countryname=" + lblCountry.Text + "'); return false;");
            }

            LinkButton cmdSendMail = (LinkButton)e.Item.FindControl("cmdSendMail");

            if (cmdSendMail != null)
            {
                cmdSendMail.Visible = SessionUtil.CanAccess(this.ViewState, cmdSendMail.CommandName);
                cmdSendMail.Attributes.Add("onclick", "openNewWindow('codehelp2','','" + Session["sitepath"] + "/Registers/RegistersCountryVisaEmail.aspx?visaid=" + lblVisaId.Text + "'); return false;");
            }

            LinkButton cmdExcel = (LinkButton)e.Item.FindControl("cmdExcel");

            if (cmdExcel != null)
            {
                cmdExcel.Visible = SessionUtil.CanAccess(this.ViewState, cmdExcel.CommandName);
                cmdExcel.Attributes.Add("onclick", "openNewWindow('codehelp3','','" + Session["sitepath"] + "/Registers/RegistersExport2XL.aspx?visaid=" + lblVisaId.Text + "'); return false;");
            }

            LinkButton  img = (LinkButton )e.Item.FindControl("imgRemarks");

            if (img != null)
            {
                img.Attributes.Add("onclick", "javascript:openNewWindow('codehelp5','', '" + Session["sitepath"] + "/Registers/RegistersCountryVisaRemarks.aspx?id=" + lblVisaId.Text + "', 'xlarge')");
                img.Visible = General.GetNullableInteger(drv["FLDSEAFARERREMARKSCOUNT"].ToString()) == 0 ? false : true;
            }
            LinkButton  imgNoRemarks = (LinkButton )e.Item.FindControl("imgNoRemarks");

            if (imgNoRemarks != null)
            {
                imgNoRemarks.Attributes.Add("onclick", "javascript:openNewWindow('codehelp6','', '" + Session["sitepath"] + "/Registers/RegistersCountryVisaRemarks.aspx?id=" + lblVisaId.Text + "', 'xlarge')");
                imgNoRemarks.Visible = General.GetNullableInteger(drv["FLDSEAFARERREMARKSCOUNT"].ToString()) == 0 ? true : false;
            }

            RadLabel lblPhysicalPresenceSpecification = (RadLabel)e.Item.FindControl("lblPhysicalPresenceSpecification");
            UserControlToolTip ucPhyPresenceTT = (UserControlToolTip)e.Item.FindControl("ucPhyPresenceTT");
            if (lblPhysicalPresenceSpecification != null)
            {
                ucPhyPresenceTT.Position = ToolTipPosition.TopCenter;
                ucPhyPresenceTT.TargetControlId = lblPhysicalPresenceSpecification.ClientID;
                //lblPhysicalPresenceSpecification.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPhyPresenceTT.ToolTip + "', 'visible');");
                //lblPhysicalPresenceSpecification.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPhyPresenceTT.ToolTip + "', 'hidden');");
            }

            RadLabel lblUrgentProcedure = (RadLabel)e.Item.FindControl("lblUrgentProcedure");
            UserControlToolTip ucUrgentProcTT = (UserControlToolTip)e.Item.FindControl("ucUrgentProcTT");
            if (lblUrgentProcedure != null)
            {
                ucUrgentProcTT.Position = ToolTipPosition.TopCenter;
                ucUrgentProcTT.TargetControlId = lblUrgentProcedure.ClientID;
                //lblUrgentProcedure.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucUrgentProcTT.ToolTip + "', 'visible');");
                //lblUrgentProcedure.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucUrgentProcTT.ToolTip + "', 'hidden');");
            }


            Guid? lblDtkey = General.GetNullableGuid(drv["FLDSEAFARERDTKEY"].ToString());
            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp4','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.REGISTERS + "');return true;");
                cmdAttachment.Visible = General.GetNullableInteger(drv["FLDSEAFARERATTACHMENTCOUNT"].ToString()) == 0 ? false : true;
            }
            LinkButton cmdNoAttachment = (LinkButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp5','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                    + PhoenixModule.REGISTERS + "');return true;");
                cmdNoAttachment.Visible = General.GetNullableInteger(drv["FLDSEAFARERATTACHMENTCOUNT"].ToString()) == 0 ? true : false;
            }

        }


    }
}
