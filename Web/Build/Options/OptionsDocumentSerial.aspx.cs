using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class OptionsDocumentSerial : PhoenixBasePage
{
 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Options/OptionsDocumentSerial.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDocumentSerial')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Options/OptionsDocumentSerial.aspx", "Find", "search.png", "FIND");

            MenuDocumentSerial.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucDocType.DocmentsTypeList = PhoenixCommanDocuments.ListDocumentType();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvDocumentSerial.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvDocumentSerial.SelectedIndexes.Clear();
        gvDocumentSerial.EditIndexes.Clear();
        gvDocumentSerial.DataSource = null;
        gvDocumentSerial.Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDOCUMENTTYPE", "FLDCOMPANYNAME", "FLDVESSELNAME", "FLDSERIAL" };
        string[] alCaptions = { "Document Type", "Company" , "Vessel", "Serial" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommanDocuments.DocumentSerialSearch(General.GetNullableInteger(ucDocType.SelectedDocumentType.ToString()), sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDocumentSerial.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentSerial.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document Serial</h3></td>");
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

    protected void MenuDocumentSerial_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDOCUMENTTYPE", "FLDCOMPANYNAME", "FLDVESSELNAME", "FLDSERIAL" };
        string[] alCaptions = { "Document Type", "Company", "Vessel", "Serial" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixCommanDocuments.DocumentSerialSearch(General.GetNullableInteger(ucDocType.SelectedDocumentType.ToString()), sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvDocumentSerial.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentSerial", "Document Serial", alCaptions, alColumns, ds);
        gvDocumentSerial.DataSource = ds;
        gvDocumentSerial.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvDocumentSerial_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDocumentType(((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany,
                    ((UserControlVessel)e.Item.FindControl("ucVesselAdd")).SelectedVessel,
                    ((UserControlMaskNumber)e.Item.FindControl("txtSerialAdd")).Text))
                   
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocumentSerial(
                        ucDocType.SelectedDocumentType,
                     ((UserControlCompany)(e.Item.FindControl("ucCompanyAdd"))).SelectedCompany,
                     ((UserControlVessel)(e.Item.FindControl("ucVesselAdd"))).SelectedVessel,
                     ((UserControlMaskNumber)(e.Item.FindControl("txtSerialAdd"))).Text


                );
                ((UserControlMaskNumber)e.Item.FindControl("txtSerialAdd")).Focus();
               
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
               if (!IsValidDocumentType(((UserControlCompany)e.Item.FindControl("ucCompanyEdit")).SelectedCompany,
                   ((UserControlVessel)e.Item.FindControl("ucVesselEdit")).SelectedVessel,
                   ((UserControlMaskNumber)e.Item.FindControl("txtSerialEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateDocumentSerial(
                         ((RadLabel)e.Item.FindControl("lblSerialEdit")).Text,
                          ucDocType.SelectedDocumentType,
                         ((UserControlCompany)(e.Item.FindControl("ucCompanyEdit"))).SelectedCompany,
                         ((UserControlVessel)(e.Item.FindControl("ucVesselEdit"))).SelectedVessel,
                         ((UserControlMaskNumber)(e.Item.FindControl("txtSerialEdit"))).Text
                     );
               
                Rebind();

            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentSerial(((RadLabel)e.Item.FindControl("lblSerialId")).Text);
               
                Rebind();
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

 
    private void InsertDocumentSerial(string documenttypeid, string companyid, string vesselid, string serialnumber)
    {
        vesselid = vesselid == "0" ? null : vesselid;
        PhoenixCommanDocuments.InsertDocumentSerial(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Convert.ToInt32(documenttypeid), General.GetNullableInteger(companyid), General.GetNullableInteger(vesselid), int.Parse(serialnumber));

    }

    private void UpdateDocumentSerial(string  serialid, string documenttypeid, string companyid, string  vesselid,string serialnumber)
    {
        vesselid = vesselid == "0" ? null : vesselid;
        PhoenixCommanDocuments.UpdateDocumentSerial(PhoenixSecurityContext.CurrentSecurityContext.UserCode,int.Parse(serialid),
            Convert.ToInt32(documenttypeid),General.GetNullableInteger(companyid), General.GetNullableInteger(vesselid), Convert.ToInt32(serialnumber));
        ucStatus.Text = "Document Serial updated";

    }

    private bool IsValidDocumentType(string documenttype, string company, string vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ucDocType.SelectedDocumentType.Equals("") || General.GetNullableInteger(ucDocType.SelectedDocumentType) == null)
            ucError.ErrorMessage = "Document type is required.";
        if (company.Equals("") || General.GetNullableInteger(company) == null)
            ucError.ErrorMessage = "Company is required.";
        if (vessel.Equals("") || General.GetNullableInteger(vessel) == null)
            ucError.ErrorMessage = "Vessel is required.";
        if (documenttype.Trim().Equals("") || General.GetNullableInteger(documenttype)==null )
            ucError.ErrorMessage = "Serial is required.";
      
        return (!ucError.IsError);
    }

    private void DeleteDocumentSerial(string  serialid)
    {
        PhoenixCommanDocuments.DeleteDocumentSerial(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(serialid));
    }

    protected void gvDocumentSerial_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
        if (e.Item is GridDataItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void gvDocumentSerial_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentSerial.CurrentPageIndex + 1;
        BindData();
    }
}
