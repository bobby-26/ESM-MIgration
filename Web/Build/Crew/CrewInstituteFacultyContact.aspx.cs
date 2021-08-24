using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CrewInstituteFacultyContact : PhoenixBasePage
{
    public enum NumberType
    {
       Home
    , Work
    , Other

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarPhone = new PhoenixToolbar();
        toolbarPhone.AddFontAwesomeButton("../Crew/CrewInstituteFacultyContact.aspx?"+ Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarPhone.AddFontAwesomeButton("javascript:CallPrint('gvFacultyPhone')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuInstituteFacultyPhone.AccessRights = this.ViewState;
        MenuInstituteFacultyPhone.MenuList = toolbarPhone.Show();

        PhoenixToolbar toolbarEmail = new PhoenixToolbar();
        toolbarEmail.AddFontAwesomeButton("../Crew/CrewInstituteFacultyContact.aspx?" + Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarEmail.AddFontAwesomeButton("javascript:CallPrint('gvFacultyEmail')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuInstituteFacultyEmail.AccessRights = this.ViewState;
        MenuInstituteFacultyEmail.MenuList = toolbarEmail.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            BindFaculty();           
            BindInstitute();            
            ViewState["CMPSTATUS"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CMP");
        }
    }
    private void BindInstitute()
    {
        string InstituteId = Request.QueryString["InstituteId"].ToString();
        DataTable dt = PhoenixCrewInstitute.CrewInstituteEdit(General.GetNullableInteger(InstituteId).Value);
        if (dt.Rows.Count > 0)
        {     
                  
            txtInstitute.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
        }
    }

    private void BindFaculty()
    {
       string instituteFacultyId = Request.QueryString["instituteFacultyId"].ToString();
        DataTable dt = PhoenixCrewInstituteFaculty.CrewInstituteFacultyEdit(General.GetNullableInteger(instituteFacultyId).Value);
        if (dt.Rows.Count > 0)
        {          
         //   MenuTitle.Title = "Contact Details  " + dt.Rows[0]["FLDFACULTYNAME"].ToString()+" - "+ dt.Rows[0]["FLDROLE"].ToString();         
        }                                                                     
    }
    private void BindPhoneData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string instituteFacultyId = Request.QueryString["instituteFacultyId"].ToString();
        string[] alColumns = { "FLDROWNUMBER", "FLDTYPENAME", "FLDPHONENO" };
        string[] alCaptions = { "S.No", "Type", "Number" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewInstituteFaculty.CrewFacultyPhoneSearch( General.GetNullableInteger(instituteFacultyId).Value
                                                                        , txtFaculty.Text.Trim()                                                                                 
                                                                        , sortexpression
                                                                        , sortdirection
                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , General.ShowRecords(null)
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount);

        General.SetPrintOptions("gvFacultyPhone", "Phone Numbers", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFacultyPhone.DataSource = ds;
            //gvFacultyPhone.DataBind();
        }
        else
        {
            gvFacultyPhone.DataSource = "";
        }
    }

    private void BindEmailData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string instituteFacultyId = Request.QueryString["instituteFacultyId"].ToString();

        string[] alColumns = { "FLDROWNUMBER", "FLDEMAIL" };
        string[] alCaptions = { "S.No", "Email" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewInstituteFaculty.CrewFacultyEmailSearch(General.GetNullableInteger(instituteFacultyId).Value
                                                                        , txtFaculty.Text.Trim()
                                                                        , sortexpression
                                                                        , sortdirection
                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , General.ShowRecords(null)
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount);

        General.SetPrintOptions("gvFacultyEmail", "Emails", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFacultyEmail.DataSource = ds;
            //gvFacultyEmail.DataBind();
        }
        else
        {
            gvFacultyEmail.DataSource = "";
        }
    }    

    //protected void gvFacultyPhone_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //        ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
    //        DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlTypeEdit");
    //        Label lblType = (Label)e.Row.FindControl("lblType");

    //        DataRowView drvType = (DataRowView)e.Row.DataItem;
    //        if (ddlType != null)
    //        {
    //            foreach (int value in Enum.GetValues(typeof(NumberType)))
    //            {
    //                ddlType.Items.Add(new ListItem(Enum.GetName(typeof(NumberType), value), value.ToString()));
    //            }
    //            ddlType.SelectedValue = drvType["FLDTYPE"].ToString();
    //        }
    //        if (cmdDelete != null)
    //        {               
    //            cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
    //            cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete this seafarer?')");
    //        }
    //        if (lblType != null)
    //        {
    //            int val = string.IsNullOrEmpty(drvType["FLDTYPE"].ToString())?3:
    //                        General.GetNullableInteger(drvType["FLDTYPE"].ToString()).Value;
    //            lblType.Text = Enum.GetName(typeof(NumberType), val);
    //        }
    //        if (eb != null)
    //            eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlTypeInsert");
    //        if (ddlType != null)
    //        {
    //            foreach (int value in Enum.GetValues(typeof(NumberType)))
    //            {
    //                ddlType.Items.Add(new ListItem(Enum.GetName(typeof(NumberType), value), value.ToString()));
    //            }
    //            //ddlType.DataBind();            
    //        }
    //    }
    //}

    //protected void gvFacultyPhone_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    string coursContactId = null, phone = null, type = null;

    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            GridView _gridView = (GridView)sender;

    //            coursContactId = Request.QueryString["instituteFacultyId"].ToString();
    //            phone = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtNumberInsert")).Text;
    //            type = ((DropDownList)_gridView.FooterRow.FindControl("ddlTypeInsert")).SelectedValue;

               
    //            if (!IsValidPhone(type, phone))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }                
    //            PhoenixCrewInstituteFaculty.CrewFacultyPhoneInsert(General.GetNullableInteger(coursContactId).Value
    //                                                                , phone
    //                                                                , type);

    //            BindPhoneData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        BindPhoneData();
    //    }
    //}

    private bool IsValidPhone(string type, string number)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(type))
            ucError.ErrorMessage = "Phone Type is required";
        if (string.IsNullOrEmpty(number))
            ucError.ErrorMessage = "Phone Number is required";        

        return (!ucError.IsError);
    }

    private bool IsValideEmail(string email)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(email))
            ucError.ErrorMessage = "Email Id is required";       

        return (!ucError.IsError);
    }
    //protected void gvFacultyPhone_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentrow = de.RowIndex;
    //        string PhoneId = _gridView.DataKeys[nCurrentrow].Value.ToString();
        
    //        PhoenixCrewInstituteFaculty.CrewFacultyPhoneDelete(General.GetNullableInteger(PhoneId).Value);
    //        _gridView.EditIndex = -1;
    //        BindPhoneData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

   
    //protected void gvFacultyPhone_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        string type = null, phone = null, instituteFacultyId = null;
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        type = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlTypeEdit")).SelectedValue;
    //        phone = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtNumberEdit")).Text;
    //        instituteFacultyId = Request.QueryString["instituteFacultyId"].ToString();
    //        string phoneId = _gridView.DataKeys[nCurrentRow].Value.ToString();
           
    //        if (!IsValidPhone(type, phone))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
            
    //        PhoenixCrewInstituteFaculty.CrewFacultyPhoneUpdate(General.GetNullableInteger(instituteFacultyId).Value
    //                                                          , General.GetNullableInteger(phoneId).Value
    //                                                          , phone
    //                                                          , type);

    //        _gridView.EditIndex = -1;
    //        BindPhoneData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvFacultyEmail_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");

    //        if (eb != null)
    //            eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

    //        if (cmdDelete != null)
    //        {
    //            cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
    //            cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete this seafarer?')");
    //        }
    //    }
    //}

    //protected void gvFacultyEmail_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    string email = null, coursecontactId = null;

    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            GridView _gridView = (GridView)sender;

    //            coursecontactId = Request.QueryString["instituteFacultyId"].ToString();
    //            email = ((TextBox)_gridView.FooterRow.FindControl("txtEmailInsert")).Text;

             
    //            if (!IsValideEmail(email))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
               
    //            PhoenixCrewInstituteFaculty.CrewFacultyEmailInsert(General.GetNullableInteger(coursecontactId).Value, email);

    //            BindEmailData();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        BindEmailData();
    //    }
    //}

    //protected void gvFacultyEmail_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentrow = de.RowIndex;
    //        string emailId = _gridView.DataKeys[nCurrentrow].Value.ToString();
         
    //        PhoenixCrewInstituteFaculty.CrewFacultyEmailDelete(General.GetNullableInteger(emailId).Value);
    //        _gridView.EditIndex = -1;
    //        BindEmailData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
   

    //protected void gvFacultyEmail_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        string email = null, instituteFacultyId = null;
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        email = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEmailEdit")).Text;
    //        instituteFacultyId = Request.QueryString["instituteFacultyId"].ToString();
    //        string emailId = _gridView.DataKeys[nCurrentRow].Value.ToString();
            
    //        if (!IsValideEmail(email))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixCrewInstituteFaculty.CrewFacultyEmailUpdate(General.GetNullableInteger(instituteFacultyId).Value
    //                                                          , General.GetNullableInteger(emailId).Value
    //                                                          , email);

    //        _gridView.EditIndex = -1;
    //        BindEmailData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void MenuInstituteFacultyPhone_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string instituteFacultyId = Request.QueryString["instituteFacultyId"].ToString();

                string[] alColumns = { "FLDROWNUMBER", "FLDTYPENAME", "FLDPHONENO" };
                string[] alCaptions = { "S.No", "Type", "Number" };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewInstituteFaculty.CrewFacultyPhoneSearch(General.GetNullableInteger(instituteFacultyId).Value
                                                                                , txtFaculty.Text.Trim()
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                , General.ShowRecords(null)
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Phone Numbers", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuInstituteFacultyEmail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDROWNUMBER", "FLDEMAIL" };
                string[] alCaptions = { "S.No", "Email" };
                string instituteFacultyId = Request.QueryString["instituteFacultyId"].ToString();
                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewInstituteFaculty.CrewFacultyEmailSearch(General.GetNullableInteger(instituteFacultyId).Value
                                                                      , txtFaculty.Text.Trim()
                                                                      , sortexpression
                                                                      , sortdirection
                                                                      , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                      , General.ShowRecords(null)
                                                                      , ref iRowCount
                                                                      , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Emails", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFacultyPhone_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string coursContactId = null, phone = null, type = null;

        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem eeditedItem = e.Item as GridFooterItem;
                coursContactId = Request.QueryString["instituteFacultyId"].ToString();
                phone = ((UserControlMobileNumber)eeditedItem.FindControl("txtNumberInsert")).Text;
                type = ((RadComboBox)eeditedItem.FindControl("ddlTypeInsert")).SelectedValue;

                if (!IsValidPhone(type, phone))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewInstituteFaculty.CrewFacultyPhoneInsert(General.GetNullableInteger(coursContactId).Value
                                                                    , phone
                                                                    , type);

                BindPhoneData();
                gvFacultyPhone.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem eeditedItem = e.Item as GridDataItem;
                string  instituteFacultyId = null;
                
                type = ((RadComboBox)eeditedItem.FindControl("ddlTypeEdit")).SelectedValue;
                phone = ((UserControlMobileNumber)eeditedItem.FindControl("txtNumberEdit")).Text;
                instituteFacultyId = Request.QueryString["instituteFacultyId"].ToString();
                string phoneId = eeditedItem.GetDataKeyValue("FLDINSTITUTEFACUTLYPHONEID").ToString();

                if (!IsValidPhone(type, phone))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewInstituteFaculty.CrewFacultyPhoneUpdate(General.GetNullableInteger(instituteFacultyId).Value
                                                                  , General.GetNullableInteger(phoneId).Value
                                                                  , phone
                                                                  , type);
                BindPhoneData();
                gvFacultyPhone.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem eeditedItem = e.Item as GridDataItem;
                string PhoneId = eeditedItem.GetDataKeyValue("FLDINSTITUTEFACUTLYPHONEID").ToString();

                PhoenixCrewInstituteFaculty.CrewFacultyPhoneDelete(General.GetNullableInteger(PhoneId).Value);
                BindPhoneData();
                gvFacultyPhone.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindPhoneData();
        }
    }

    protected void gvFacultyPhone_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindPhoneData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFacultyPhone_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            int rowCounter;
            RadLabel lblRowNo = e.Item.FindControl("lblRowNo") as RadLabel;
            rowCounter = gvFacultyPhone.MasterTableView.PageSize * gvFacultyPhone.MasterTableView.CurrentPageIndex;
            if (lblRowNo != null)
                lblRowNo.Text = (e.Item.ItemIndex + 1 + rowCounter).ToString();

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            RadComboBox ddlType = (RadComboBox)e.Item.FindControl("ddlTypeEdit");
            RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");

            DataRowView drvType = (DataRowView)e.Item.DataItem;
            if (ddlType != null)
            {
                foreach (int value in Enum.GetValues(typeof(NumberType)))
                {
                    ddlType.Items.Add(new RadComboBoxItem(Enum.GetName(typeof(NumberType), value), value.ToString()));
                }
                ddlType.SelectedValue = drvType["FLDTYPE"].ToString();
            }
            if (cmdDelete != null)
            {
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete this seafarer?')");
            }
            if (lblType != null)
            {
                int val = string.IsNullOrEmpty(drvType["FLDTYPE"].ToString()) ? 3 :
                            General.GetNullableInteger(drvType["FLDTYPE"].ToString()).Value;
                lblType.Text = Enum.GetName(typeof(NumberType), val);
            }
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlType = (RadComboBox)e.Item.FindControl("ddlTypeInsert");
            if (ddlType != null)
            {
                foreach (int value in Enum.GetValues(typeof(NumberType)))
                {
                    ddlType.Items.Add(new RadComboBoxItem(Enum.GetName(typeof(NumberType), value), value.ToString()));
                }          
            }
        }
    }

    protected void gvFacultyEmail_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string email = null, coursecontactId = null;

        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem eeditedItem = e.Item as GridFooterItem;
                coursecontactId = Request.QueryString["instituteFacultyId"].ToString();
                email = ((RadTextBox)eeditedItem.FindControl("txtEmailInsert")).Text;

                if (!IsValideEmail(email))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewInstituteFaculty.CrewFacultyEmailInsert(General.GetNullableInteger(coursecontactId).Value, email);
                BindEmailData();
                gvFacultyEmail.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem eeditedItem = e.Item as GridDataItem;
                string  instituteFacultyId = null;
                email = ((RadTextBox)eeditedItem.FindControl("txtEmailEdit")).Text;
                instituteFacultyId = Request.QueryString["instituteFacultyId"].ToString();
                string emailId = eeditedItem.GetDataKeyValue("FLDINSTITUTEFACUTLYEMAILID").ToString();

                if (!IsValideEmail(email))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewInstituteFaculty.CrewFacultyEmailUpdate(General.GetNullableInteger(instituteFacultyId).Value
                                                                  , General.GetNullableInteger(emailId).Value
                                                                  , email);
               
                BindEmailData();
                gvFacultyEmail.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem eeditedItem = e.Item as GridDataItem;
                string emailId = eeditedItem.GetDataKeyValue("FLDINSTITUTEFACUTLYEMAILID").ToString();
                PhoenixCrewInstituteFaculty.CrewFacultyEmailDelete(General.GetNullableInteger(emailId).Value);             
                BindEmailData();
                gvFacultyEmail.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            //BindEmailData();
        }
    }

    protected void gvFacultyEmail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindEmailData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFacultyEmail_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            int rowCounter;
            RadLabel lblRowNo = e.Item.FindControl("lblRowNo") as RadLabel;
            rowCounter = gvFacultyPhone.MasterTableView.PageSize * gvFacultyPhone.MasterTableView.CurrentPageIndex;
            if (lblRowNo != null)
                lblRowNo.Text = (e.Item.ItemIndex + 1 + rowCounter).ToString();

            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");

            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            if (cmdDelete != null)
            {
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete this seafarer?')");
            }
        }
    }
}