using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewReference : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();            
            toolbar.AddFontAwesomeButton("../Crew/CrewReference.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewReference')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuReference.AccessRights = this.ViewState;
            MenuReference.MenuList = toolbar.Show();
            
            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            CrewTab.AccessRights = this.ViewState;
            CrewTab.MenuList = toolbar2.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;                
                SetEmployeePrimaryDetails();
                gvCrewReference.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReference_TabStripCommand(object sender, EventArgs e)
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
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCOMPANYNAME", "FLDPIC", "FLDDESIGNATION", "FLDPHONENUMBER", "FLDCOMMENTS" };
        string[] alCaptions = { "Company name", "Person in Charge", "Designation", "Phone Number", "Comments" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReference.CrewReferenceSearch(
                        Int32.Parse(Filter.CurrentCrewSelection.ToString())
                        , sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Reference", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPANYNAME", "FLDPIC", "FLDDESIGNATION", "FLDPHONENUMBER", "FLDCOMMENTS" };
            string[] alCaptions = { "Company name", "Person in Charge", "Designation", "Phone Number", "Comments" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewReference.CrewReferenceSearch(
                        Int32.Parse(Filter.CurrentCrewSelection.ToString())
                        , sortexpression, sortdirection
                        ,int.Parse(ViewState["PAGENUMBER"].ToString())
                        ,gvCrewReference.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            General.SetPrintOptions("gvCrewReference", "Reference", alCaptions, alColumns, ds);

            gvCrewReference.DataSource = ds;
            gvCrewReference.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewReference_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewReference.CurrentPageIndex + 1;

        BindData();
    }


    protected void gvCrewReference_ItemCommand(object sender, GridCommandEventArgs e)
    {
     
        if (e.CommandName.ToString().ToUpper() == "ADD")
        {
            try
            {
                string companyname = ((RadTextBox)e.Item.FindControl("txtCompanyNameAdd")).Text;
                string pic = ((RadTextBox)e.Item.FindControl("txtPicAdd")).Text;
                string designation = ((RadTextBox)e.Item.FindControl("txtDesignationAdd")).Text;
                string phonenumber = ((UserControlPhoneNumber)e.Item.FindControl("txtPhoneNumberAdd")).Text;
                string comments = ((RadTextBox)e.Item.FindControl("txtCommentsAdd")).Text;
                string dateofreference = ((UserControlDate)e.Item.FindControl("txtReferenceDateAdd")).Text;
                string refferredby = ((UserControlQuick)e.Item.FindControl("ucRefferredByAdd")).SelectedQuick;

                if (!IsValidReference(companyname, dateofreference))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewReference.InsertCrewReference(Convert.ToInt32(Filter.CurrentCrewSelection)
                    , companyname
                    , pic
                    , designation
                    , phonenumber
                    , comments
                    , General.GetNullableDateTime(dateofreference)
                    , General.GetNullableInteger(refferredby)
                    );

                BindData();
                gvCrewReference.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewReference_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string referenceid = ((RadLabel)e.Item.FindControl("lblReferenceEdit")).Text;
            string companyname = ((RadTextBox)e.Item.FindControl("txtCompanyName")).Text;
            string pic = ((RadTextBox)e.Item.FindControl("txtPic")).Text;
            string designation = ((RadTextBox)e.Item.FindControl("txtDesignation")).Text;
            string phonenumber = ((UserControlPhoneNumber)e.Item.FindControl("txtPhoneNumber")).Text;
            string comments = ((RadLabel)e.Item.FindControl("lblRemarksEdit")).Text;
            string dateofreference = ((UserControlDate)e.Item.FindControl("txtReferenceDateEdit")).Text;
            string refferredby = ((UserControlQuick)e.Item.FindControl("ucRefferredByEdit")).SelectedQuick;
            if (!IsValidReference(companyname, dateofreference))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewReference.UpdateCrewReference(Convert.ToInt32(referenceid)
                , companyname
                , pic
                , designation
                , phonenumber
                , comments
                , General.GetNullableDateTime(dateofreference)
                , General.GetNullableInteger(refferredby)
               );

            BindData();
            gvCrewReference.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }     
    }

    protected void gvCrewReference_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string referenceid = ((RadLabel)e.Item.FindControl("lblReferenceId")).Text;

            PhoenixCrewReference.DeleteCrewReference(Convert.ToInt32(referenceid));
            BindData();
            gvCrewReference.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void gvCrewReference_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
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
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.REFERENCE + "&cmdname=REFERENCEUPLOAD'); return false;");
                
            }            
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblReferenceId");
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
                img.Attributes.Add("onclick", "javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&edityn=1&type=" + PhoenixCrewAttachmentType.REFERENCE + "','xlarge')");
            }            
        }
        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;          
            UserControlQuick ucRefferredByEdit = (UserControlQuick)e.Item.FindControl("ucRefferredByEdit");
            if (ucRefferredByEdit != null)
            {
                ucRefferredByEdit.QuickList = PhoenixRegistersQuick.ListQuick(1, 127);
                ucRefferredByEdit.DataBind();
                ucRefferredByEdit.SelectedQuick = drv["FLDREFFERREDBY"].ToString();
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            
            UserControlQuick ucRefferredByAdd = (UserControlQuick)e.Item.FindControl("ucRefferredByAdd");
            if (ucRefferredByAdd != null)
            {
                ucRefferredByAdd.QuickList = PhoenixRegistersQuick.ListQuick(1, 127);
                ucRefferredByAdd.DataBind();
            }
            
        }
    }

    private bool IsValidReference(string companyname, string dateofreference)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (companyname.Trim() == string.Empty)
            ucError.ErrorMessage = "Company Name is required";

        if (string.IsNullOrEmpty(dateofreference) == false)
        {
            if (DateTime.TryParse(dateofreference, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
                ucError.ErrorMessage = "Date of Reference Should not be a Future Date";
        }
        return (!ucError.IsError);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
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
        BindData();
        gvCrewReference.Rebind();
    }

}
