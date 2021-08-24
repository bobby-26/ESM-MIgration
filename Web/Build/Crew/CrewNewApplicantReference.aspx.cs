using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewNewApplicantReference : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantReference.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewReference')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuReference.AccessRights = this.ViewState;
            MenuReference.MenuList = toolbar.Show();

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
                SetEmployeePrimaryDetails();
                gvCrewReference.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();
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
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
                        , sortexpression, sortdirection
                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrewReference.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);       

        Response.AddHeader("Content-Disposition", "attachment; filename=Reference.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Reference</h3></td>");
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
                        Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
                        , sortexpression, sortdirection
                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrewReference.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            General.SetPrintOptions("gvCrewReference", "Reference", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewReference.DataSource = ds;
                gvCrewReference.VirtualItemCount = iRowCount;
            }
            else
            {
                gvCrewReference.DataSource = "";
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewReference_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidReference(string companyname,string dateofreference)
    {      
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

       if(companyname.Trim() == string.Empty)
           ucError.ErrorMessage = "Company Name is required";
       //if (phonenumber.Trim() == string.Empty)
       //    ucError.ErrorMessage = "Phone Number is required";
       if (string.IsNullOrEmpty(dateofreference)==false)
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
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
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

    protected void gvCrewReference_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToString().ToUpper() == "ADD")
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
                PhoenixCrewReference.InsertCrewReference(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
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
            else if (e.CommandName.ToString().ToUpper() == "UPDATE")
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
            else if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string referenceid = ((RadLabel)e.Item.FindControl("lblReferenceId")).Text;

                PhoenixCrewReference.DeleteCrewReference(Convert.ToInt32(referenceid));
                BindData();
                gvCrewReference.Rebind();
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

    protected void gvCrewReference_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewReference.CurrentPageIndex + 1;
            BindData();
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
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.REFERENCE + "&cmdname=NAPPREFUPLOAD'); return false;");
            }
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblReferenceId");
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (img != null)
            {
                img.Attributes.Add("onclick", "parent.Openpopup('MoreInfo','','CrewMoreInfo.aspx?id=" + lbl.Text + "&edityn=1&type=" + PhoenixCrewAttachmentType.REFERENCE + "','xlarge')");
                if (string.IsNullOrEmpty(lblR.Text.Trim()))
                {
                    HtmlGenericControl html = new HtmlGenericControl();                    
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                    img.Controls.Add(html);
                } 
            }
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
            UserControlQuick ucRefferredByAdd = (UserControlQuick)e.Item.FindControl("ucRefferredByAdd");
            if (ucRefferredByAdd != null)
            {
                ucRefferredByAdd.QuickList = PhoenixRegistersQuick.ListQuick(1, 127);
                ucRefferredByAdd.DataBind();
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrewReference.Rebind();
    }
}
