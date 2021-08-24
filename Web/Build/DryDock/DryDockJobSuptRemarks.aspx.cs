using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DryDock;
using Telerik.Web.UI;


public partial class DryDockJobSuptRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Project", "PROJECT");
        toolbar.AddButton("Quotation", "QUOTATION");
        toolbar.AddButton("Supt Job Remarks", "JOBREMARKS");
        toolbar.AddButton("Supt Feedback", "SUPTFEEDBACK");




        MenuSuptRemark.AccessRights = this.ViewState;
        MenuSuptRemark.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../DryDock/DryDockJobSuptRemarks.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("../DryDock/DryDockJobSuptRemarks.aspx?" + Request.QueryString.ToString(), "Export to Word", "<i class=\"fas fa-file-word\"></i>", "HTMLREPORT");
        MenuRemarks.AccessRights = this.ViewState;
        MenuRemarks.MenuList = toolbar.Show();
        
        MenuSuptRemark.SelectedMenuIndex = 2;
       

        if (!IsPostBack)
        {
            BindData();

        }
    }
    
    protected void MenuSuptRemark_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("QUOTATION"))
            {
                if (Filter.CurrentSelectedDryDockProject != null)
                    Response.Redirect("../DryDock/DryDockQuotation.aspx?vslid=" + Request.QueryString["vslid"], false);
            }
            if (CommandName.ToUpper().Equals("PROJECT"))
            {
                Response.Redirect("../DryDock/DryDockProject.aspx", false);
            }
            if (CommandName.ToUpper().Equals("SUPTFEEDBACK"))
            {
                if (Filter.CurrentSelectedDryDockProject != null)
                    Response.Redirect("../DryDock/DryDockSuptRemarks.aspx?" + Request.QueryString.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            if (Request.QueryString["orderid"] != null)
            {
                DataSet ds = PhoenixDryDockJob.ListDryDockJobSuptRemarks(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["orderid"]));
                Response.ClearContent();
                Response.Write(SuptJobRemarks(ds));
                Response.AddHeader("Content-Disposition", "attachment; filename=Supt_Job_Remarks.xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.End();
            }
        }

        if (CommandName.ToUpper().Equals("HTMLREPORT"))
        {
            if (Request.QueryString["orderid"] != null)
            {
                DataSet ds = PhoenixDryDockJob.ListDryDockJobSuptRemarks(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["orderid"]));
                Response.ClearContent();
                Response.Write(SuptJobRemarks(ds));
                Response.ContentType = "application/msword";
                Response.AddHeader("Content-Disposition", "attachment; filename=Supt_Job_Remarks.doc");
                Response.End();
            }
        }        
    }
    private void BindData()
    {
        try
        {
            if (Request.QueryString["orderid"] != null)
            {
                DataSet ds = PhoenixDryDockJob.ListDryDockJobSuptRemarks(int.Parse(Request.QueryString["vslid"]), new Guid(Request.QueryString["orderid"]));
                divContainer.InnerHtml = SuptJobRemarks(ds);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string SuptJobRemarks(DataSet ds)
    {
        DataTable dt = ds.Tables[0];
        DataTable dt1 = ds.Tables[1];
        DataRow[] drs = null;
        string template = @"
                <tr>
                    <th>
                        Job No
                    </th>
                    <td>
                        {0}
                    </td>
                    <th>
                        Job Type
                    </th>                    
                    <td>
                         {1}
                    </td>
                    <th>
                        Job Title
                    </th>
                    <td>                       
                         {2}
                    </td>                    
                </tr>
                <tr>
                    <th>
                        Components
                    </th>
                    <td colspan=""5"">    
                        {3}
                    </td>
                </tr>
                <tr>
                    <th colspan=""6"" style=""text-align:left"">
                        Comments
                    </th>
                </tr>";

        string tempjobid = string.Empty;
        string table = "<table cellspacing=\"0\" cellpadding=\"3\" border=\"0\" style=\"font-size: 11px; width: 100%;\" id=\"gvSuptRemark\">";
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            DataRow dr = dt.Rows[i];
            string jobid = dr["FLDJOBID"].ToString();
            if (tempjobid == jobid) continue;
            drs = dt.Select("FLDJOBID = '" + jobid + "'");
            if (drs.Length > 0 && drs[0]["FLDSUPTREMARKS"].ToString().Equals(""))
            {
                continue;
            }
            string component = string.Empty;

            drs = dt1.Select("FLDJOBID = '" + jobid + "'");
            if (drs.Length > 0)
            {
                foreach (DataRow drv in drs)
                {
                    component = string.Concat(component, drv["FLDCOMPONENTNUMBER"].ToString() + " - " + drv["FLDCOMPONENTNAME"].ToString() + ", ");
                }
            }
            component = component.Trim().TrimEnd(',');

            table = string.Concat(table, string.Format(template, dr["FLDJOBNUMBER"].ToString(), dr["FLDJOBTYPE"].ToString(), dr["FLDJOBTITLE"].ToString(), component));

            drs = dt.Select("FLDJOBID = '" + jobid + "'");
            if (drs.Length > 0)
            {
                foreach (DataRow drv in drs)
                {
                    table = string.Concat(table, "<tr>");
                    table = string.Concat(table, "<td> Date : <b>");
                    table = string.Concat(table, String.Format("{0:dd/MM/yyyy}", drv["FLDCREATEDDATE"]));
                    table = string.Concat(table, "</b></td>");
                    table = string.Concat(table, "<td colspan=\"5\">");
                    table = string.Concat(table, drv["FLDSUPTREMARKS"].ToString() + "<br></br> - <b>" + drv["FLDUSERNAME"].ToString() + "</b>");
                    table = string.Concat(table, "</td>");
                    table = string.Concat(table, "</tr>");
                }
            }
            else
            {
                table = string.Concat(table, "<tr>");
                table = string.Concat(table, "<td colspan=\"6\">");
                table = string.Concat(table, "There are no comments for this job");
                table = string.Concat(table, "</td>");
                table = string.Concat(table, "</tr>");
            }
            table = string.Concat(table, "<tr><td colspan=\"6\"><hr/></td></tr>");
            tempjobid = jobid;
        }
        table = string.Concat(table, "</table>");

        return table;
    }
   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
