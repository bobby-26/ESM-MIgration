using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Common_CommonPickListCrewChangePlan : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Search", "GO",ToolBarDirection.Right);
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        MenuOrderLine.AccessRights = this.ViewState;
        MenuOrderLine.MenuList = toolbar.Show();
        //MenuOrderLine.SetTrigger(pnlUserEntry);
       // binddata();
    }


    public void binddata()
    {
        try
        {
            DataTable dt = new DataTable();
            string rankid="";
            if (General.GetNullableInteger(ddlRank.SelectedRank) == null)
            {
                rankid = Request.QueryString["rankid"];
                ddlRank.SelectedValue = int.Parse(Request.QueryString["rankid"]);
            }
             else
                rankid = ddlRank.SelectedValue.ToString();



            dt = PhoenixInspectionPNI.CrewChangePlanList(General.GetNullableInteger(Request.QueryString["vesselid"])
                                                         , General.GetNullableInteger(rankid)
                                                         , General.GetNullableDateTime(txtfromdate.Text)
                                                         , General.GetNullableDateTime(txttodate.Text));

            gvcrewplan.DataSource = dt;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
  
   
    protected void MenuOrderLine_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GO"))
            {
                binddata();
            }
               
      
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvcrewplan_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        binddata();
    }

    protected void gvcrewplan_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
        string Script = "";
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = new NameValueCollection();

            LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName");
            nvc.Add(lnkName.ID, lnkName.Text);
            RadLabel lb = (RadLabel)e.Item.FindControl("lblcrewplanid");
            nvc.Add(lb.ID, lb.Text.ToString());
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblemployeeid");
            nvc.Add(lbl.ID, lbl.Text);
            RadLabel lblrankid = (RadLabel)e.Item.FindControl("lblrankid");
            nvc.Add(lblrankid.ID, lblrankid.Text);
        }
        else
        {

            nvc = Filter.CurrentPickListSelection;

            LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName");
            nvc.Set(nvc.GetKey(1), lnkName.Text);
            RadLabel lblcrewplanid = (RadLabel)e.Item.FindControl("lblcrewplanid");
            nvc.Set(nvc.GetKey(2), lblcrewplanid.Text.ToString());
            RadLabel lblemployeeid = (RadLabel)e.Item.FindControl("lblemployeeid");
            nvc.Set(nvc.GetKey(3), lblemployeeid.Text);
            RadLabel lblrankid = (RadLabel)e.Item.FindControl("lblrankid");
            nvc.Set(nvc.GetKey(4), lblrankid.Text);
            //RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
            //nvc.Set(nvc.GetKey(4), lblBudgetGroupId.Text);

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
            else
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

        }



        Filter.CurrentPickListSelection = nvc;

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
