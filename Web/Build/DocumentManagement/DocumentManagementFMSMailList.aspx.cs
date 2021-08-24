using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using System.IO;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using Telerik.Web.UI;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;

public partial class DocumentManagementFMSMailList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbar.AddButton("E Mails", "ESMA", ToolBarDirection.Left);
                toolbar.AddButton("ESM Filing", "ESMF", ToolBarDirection.Left);
            }
            toolbar.AddButton("Shipboard Forms", "SPFF", ToolBarDirection.Left);

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                toolbar.AddButton("Office Forms", "OFFF", ToolBarDirection.Left);

            toolbar.AddButton("Maintenance Forms", "MCFS", ToolBarDirection.Left);
            toolbar.AddButton("Plans and Drawings", "DRWS", ToolBarDirection.Left);
            toolbar.AddButton("Manuals", "MNSF", ToolBarDirection.Left);
            toolbar.AddButton("Equipment Manuals", "EQSF", ToolBarDirection.Left);
            MenuFMS.AccessRights = this.ViewState;
            MenuFMS.MenuList = toolbar.Show();
            MenuFMS.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarmail = new PhoenixToolbar();
            toolbarmail.AddButton("File Number", "FILEN", ToolBarDirection.Right);
            toolbarmail.AddButton("Inbox", "INB", ToolBarDirection.Right);

            PhoenixToolbar toolbarmenu = new PhoenixToolbar();
            toolbarmenu.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSMailList.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarmenu.AddFontAwesomeButton("../DocumentManagement/DocumentManagementFMSMailList.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuFMSEmail.AccessRights = this.ViewState;
            MenuFMSEmail.MenuList = toolbarmenu.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvFMSEmail.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindFileNos();
                if (string.IsNullOrEmpty(Request.QueryString["callfrom"]) == false)
                {
                    filterselection();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void filterselection()
    {
        NameValueCollection nvc = Filter.CurrentFMSMAILFilterSelection;

        if (nvc != null)
        {
            ddlfileno.SelectedValue = nvc.Get("txtFileNumber").ToString();
            ddlVessel.SelectedVessel = nvc.Get("txtvesselid").ToString();
            txtFromDate.Text = nvc.Get("txtFromDate");
            txtToDate.Text = nvc.Get("txtToDate"); 
            txtSender.Text = nvc.Get("txtSender").ToString();
            txtRecipient.Text = nvc.Get("txtRecipient").ToString();
            txtSubject.Text = nvc.Get("txtSubject").ToString();
        }
    }

    
    private void BindFileNos()
    {
        DataSet ds = PhoenixDocumentManagementFMSReports.FMSMailFilenoList();
        ddlfileno.DataSource = ds;        
        ddlfileno.DataBind();
        ddlfileno.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //gvFMSEmail.CurrentPageIndex = 0;
        //gvFMSEmail.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        NameValueCollection nvc = Filter.CurrentFMSMAILFilterSelection;

        //if (ddlVessel.SelectedVessel != "")
        //{
        //    DataSet ds = PhoenixRegistersVesselCommunicationDetails.EditCommunicationDetails(ddlVessel.SelectedValue);
        //    if (ds.Tables[0].Rows.Count > 0)
        //        txtSender.Text = ds.Tables[0].Rows[0]["FLDEMAIL"].ToString();
        //}

        DataTable dt = new DataTable();

        if (Filter.CurrentFMSMAILFilterSelection != null)
        {
            dt = PhoenixDocumentManagementFMSReports.ListFMSEmail(General.GetNullableString(nvc.Get("txtFileNumber").ToString())
                       , General.GetNullableInteger(nvc.Get("txtvesselid").ToString())
                       , General.GetNullableString(nvc.Get("txtSubject").ToString())
                       , General.GetNullableDateTime(nvc.Get("txtFromDate"))
                       , General.GetNullableDateTime(nvc.Get("txtToDate"))
                       , General.GetNullableString(nvc.Get("txtSender").ToString())
                       , General.GetNullableString(nvc.Get("txtRecipient").ToString())
                       , gvFMSEmail.CurrentPageIndex + 1
                       , gvFMSEmail.PageSize
                       , ref iRowCount
                       , ref iTotalPageCount);
        }
        else
        {
            dt = PhoenixDocumentManagementFMSReports.ListFMSEmail(null
              , null
              , null
              , null
              , null
              , null
              , null
              , gvFMSEmail.CurrentPageIndex + 1
              , gvFMSEmail.PageSize
              , ref iRowCount
              , ref iTotalPageCount);
        }

        gvFMSEmail.DataSource = dt;
        gvFMSEmail.VirtualItemCount = iRowCount;

       // ViewState["ROWCOUNT"] = iRowCount;
        //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void MenuFMS_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ESMA"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMailList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("ESMF"))
            {
                Response.Redirect("../DocumentManagement/DocumentFMSFileNoList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("SPFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSShipboardFormList.aspx?CATEGORYNO=2", false);
            }
            if (CommandName.ToUpper().Equals("OFFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSOfficeFormList.aspx?CATEGORYNO=16&Callfrom=1", false);
            }
            if (CommandName.ToUpper().Equals("MCFS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMaintenanceHistoryTemplate.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("DRWS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSDrawingList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("MNSF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSVesselSurveyScheduleList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("EQSF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSEquipmentManuals.aspx?", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuFMSEmail_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            string Script = "";
            Script += "<script language='JavaScript' id='BookMarkScript'>";
            Script += "fnReloadList();";
            Script += "</script>";

            ViewState["PAGENUMBER"] = 1;
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();

            criteria.Add("txtFileNumber", ddlfileno.SelectedValue);
            criteria.Add("txtvesselid", ddlVessel.SelectedVessel);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);
            criteria.Add("txtSender", txtSender.Text);
            criteria.Add("txtRecipient", txtRecipient.Text);
            criteria.Add("txtSubject", txtSubject.Text);

            Filter.CurrentFMSMAILFilterSelection = criteria;

            gvFMSEmail.CurrentPageIndex = 0;
            Rebind();
        }

        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ddlfileno.ClearSelection();
            ddlVessel.SelectedVessel = "";
            txtSubject.Text = null;
            txtFromDate.Text = null;
            txtToDate.Text = null;
            txtSender.Text = null;
            txtRecipient.Text = null;
            Filter.CurrentFMSMAILFilterSelection = null;
            ViewState["PAGENUMBER"] = 1;

            Rebind();
          
        }
    }

    protected void gvFMSEmail_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SUBJECT")
            {
                string mailid = ((RadLabel)e.Item.FindControl("lblMailid")).Text;
                string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMailRead.aspx?Mailid=" + mailid, false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvFMSEmail_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton Subject = (LinkButton)e.Item.FindControl("lnkSubject");

            RadLabel Dtkey = (RadLabel)e.Item.FindControl("lbldtkey");
            RadLabel Mailid = (RadLabel)e.Item.FindControl("lblMailid");

            LinkButton lblmailsubject = (LinkButton)e.Item.FindControl("lnkSubject");
            if (lblmailsubject != null)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipKeyword");
                //lbladdressName.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                //lbladdressName.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblmailsubject.ClientID;
            }
            //lnk.Attributes.Add("onclick", "javascript:Openpopup('Codehelp1','','../Common/download.aspx?dtkey=" + Dtkey.Text + "'); return false;");
        }
    }

    protected void gvFMSEmail_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
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

    protected void gvFMSEmail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            // ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : RadGrid1.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvFMSEmail.SelectedIndexes.Clear();
        gvFMSEmail.EditIndexes.Clear();
        gvFMSEmail.DataSource = null;
        gvFMSEmail.Rebind();
    }

}
