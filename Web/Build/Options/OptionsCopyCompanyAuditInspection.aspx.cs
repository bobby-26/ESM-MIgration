using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;


public partial class Options_OptionsCopyCompanyAuditInspection : PhoenixBasePage
{

   protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Options/OptionsCopyCompanyAuditInspection.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCopyInspection')", "Print Grid", "icon_print.png", "PRINT");

            MenuCopyInspection.AccessRights = this.ViewState;
            MenuCopyInspection.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (Filter.CurrentSelectedInspection != null)
                    Filter.CurrentSelectedInspection = null;

              //  ucConfirmDelete.Visible = false;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["INSPECTIONID"] = "";
                gvCopyInspection.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
         
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvCopyInspection.SelectedIndexes.Clear();
        gvCopyInspection.EditIndexes.Clear();
        gvCopyInspection.DataSource = null;
        gvCopyInspection.Rebind();
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOMPANYNAME", "FLDINSPECTIONCATEGORYNAME", "FLDSHORTCODE", "FLDINSPECTIONNAME", "FLDACTIVEYN", 
                                 "FLDFREQUENCYINMONTHS", "FLDADDTOSCHEDULE", "FLDOFFICEYNNAME" , "FLDWINDOWPERIODBEFORE" , "FLDWINDOWPERIODAFTER"};
        string[] alCaptions = { "Company", "Category", "Short Code", "Name", "Active Y/N", "Frequency (in months)", "Add to Schedule",
                                  "Office Audit Y/N" , "Window Before" , "Window After"};

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (!IsPostBack)
            ViewState["SELECTEDCOMPANY"] = 4;
        else
            ViewState["SELECTEDCOMPANY"] = ucSourceCompany.SelectedCompany;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixInspection.ListCompanyInspection(
          General.GetNullableInteger(ViewState["SELECTEDCOMPANY"].ToString())
         , sortexpression, sortdirection
         , Int32.Parse(ViewState["PAGENUMBER"].ToString())
         , General.ShowRecords(null)
         , ref iRowCount
         , ref iTotalPageCount
         );

        Response.AddHeader("Content-Disposition", "attachment; filename=InspectionList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3> Inspection List </h3></td>");
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void MenuCopyInspection_TabStripCommand(object sender, EventArgs e)
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


    protected void btnCopy_Click(object sender, EventArgs e)
    {
        if (ucSourceCompany.SelectedCompany.ToString() == ucDestinationCompany.SelectedCompany.ToString())
        {
            ucError.ErrorMessage = "Source and destination should not be the same";
            ucError.Visible = true;
            return;
        }
        else if (Filter.CurrentSelectedInspection == null)
        {
            ucError.ErrorMessage = "Please select the inspections";
            ucError.Visible = true;
            return;
        }
     //   ucConfirmDelete.Visible = true;
     //   ucConfirmDelete.Text = "Are you sure you want to copy all the selected inspection.? Y/N";
        return;     
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOMPANYNAME", "FLDINSPECTIONCATEGORYNAME", "FLDSHORTCODE", "FLDINSPECTIONNAME", "FLDACTIVEYN", 
                                 "FLDFREQUENCYINMONTHS", "FLDADDTOSCHEDULE", "FLDOFFICEYNNAME" , "FLDWINDOWPERIODBEFORE" , "FLDWINDOWPERIODAFTER" };
        string[] alCaptions = { "Company", "Category", "Short Code", "Name", "Active Y/N", "Frequency (in months)", "Add to Schedule",
                                  "Office Audit Y/N" , "Window Before" , "Window After"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (!IsPostBack)
            ViewState["SELECTEDCOMPANY"] = 4;
        else        
            ViewState["SELECTEDCOMPANY"] = ucSourceCompany.SelectedCompany;


        DataSet ds = PhoenixInspection.ListCompanyInspection(
          General.GetNullableInteger(ViewState["SELECTEDCOMPANY"].ToString())
         , sortexpression, sortdirection
         , Int32.Parse(ViewState["PAGENUMBER"].ToString())
         , General.ShowRecords(null)
         , ref iRowCount
         , ref iTotalPageCount
         );

        General.SetPrintOptions("gvCopyInspection", "Insection List", alCaptions, alColumns, ds);

        gvCopyInspection.DataSource = ds;
        gvCopyInspection.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;


    }

    protected void gvCopyInspection_ItemDataBound(Object sender, GridItemEventArgs e)
    {



        if (e.Item is GridEditableItem)
        {
            DataRowView dv = (DataRowView)e.Item.DataItem;
            Button copy = (Button)e.Item.FindControl("btnCopy");
            CheckBox chkSelect = (CheckBox)e.Item.FindControl("chkSelect");

            if (copy != null && dv["FLDARCHIVEDYN"].ToString() == "1")
            {
                copy.Visible = false;
                chkSelect.Visible = false;
            }

            if (copy != null && !SessionUtil.CanAccess(this.ViewState, copy.CommandName))
                copy.Visible = false;
        }
    }

   
   
   

    //private void SetRowSelection()
    //{
       
    //    for (int i = 0; i < gvCopyInspection.Row.Count; i++)
    //    {
    //        if (gvCopyInspection.DataKeys[i].Value.ToString().Equals(ViewState["INSPECTIONID"].ToString()))
    //        {
    //            gvCopyInspection.SelectedIndex = i;
    //        }
    //    }
    //}

   

    //public StateBag ReturnViewState()
    //{
    //    return ViewState;
    //}

    public void ucSourceCompany_Changed(object sender, EventArgs e)
    {
        ViewState["SELECTEDCOMPANY"] = Convert.ToInt32(ucSourceCompany.SelectedCompany);
        BindData();
    }
   

    //protected void ucConfirmInspection_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

    //        if (ucCM.confirmboxvalue == 1)
    //        {
    //            if (Filter.CurrentSelectedInspection != null)
    //            {
    //                ArrayList selectedInspection = (ArrayList)Filter.CurrentSelectedInspection;
    //                if (selectedInspection != null && selectedInspection.Count > 0)
    //                {
    //                    foreach (Guid inspectionid in selectedInspection)
    //                    {
    //                            PhoenixInspection.CopyCompanyInspection(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
    //                                                    General.GetNullableInteger(ViewState["SELECTEDCOMPANY"].ToString()),
    //                                                    General.GetNullableInteger(ucDestinationCompany.SelectedCompany),
    //                                                    General.GetNullableGuid(inspectionid.ToString()));
    //                    }
    //                }
    //            }
    //            Filter.CurrentSelectedInspection = null;
    //            ucStatus.Text = "Inspections are copied.";
    //            BindData();

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        return;
    //    }
    //}

    //protected void CheckAll(Object sender, EventArgs e)
    //{
    //    string[] ctl = Request.Form.GetValues("__EVENTTARGET");

    //    if (ctl != null && ctl[0].ToString() == "gvCopyInspection$ctl01$chkAllInspection")
    //    {
    //        CheckBox chkAll = (CheckBox)gvCopyInspection.HeaderRow.FindControl("chkAllInspection");
    //        foreach (GridViewRow row in gvCopyInspection.Item)
    //        {
    //            CheckBox cbSelected = (CheckBox)row.FindControl("chkSelect");
    //            if (chkAll.Checked == true)
    //            {
    //                cbSelected.Checked = true;
    //            }
    //            else
    //            {
    //                cbSelected.Checked = false;
    //                Filter.CurrentSelectedInspection = null;
    //            }
    //        }
    //    }
    //}



    protected void gvCopyInspection_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  

    protected void gvCopyInspection_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCopyInspection.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   // protected void gvCopyInspection_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
   // {
   //     ViewState["SORTEXPRESSION"] = e.SortExpression;
   //     switch (e.NewSortOrder)
   //     {
   //         case GridSortOrder.Ascending:
   //             ViewState["SORTDIRECTION"] = "0";
   //             break;
   //         case GridSortOrder.Descending:
   //             ViewState["SORTDIRECTION"] = "1";
   //             break;
   //     }
   // }
}
