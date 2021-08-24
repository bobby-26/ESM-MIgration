using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;
public partial class CommonPickListMedicalCase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuAccount.Title = "Medical Case";
        MenuAccount.MenuList = toolbarmain.Show();
  //      MenuAccount.SetTrigger(pnlAccount);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvmedical.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


        }
        //     BindData();

    }
    protected void gvmedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvmedical.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAccount_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
              //  SetPageNavigator();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (Request.QueryString["Vesselid"] != null && Request.QueryString["Vesselid"].ToString() !="")
            {
                ViewState["VESSELID"] = Request.QueryString["Vesselid"];
               // ddlvessel.SelectedVessel = ViewState["VESSELID"].ToString();
            }

            //ucError.ErrorMessage = ddlvessel.SelectedVessel.ToString();
            //ucError.Visible = true;
            ds = PhoenixInspectionPNI.PNIMedicalCaseSearch
                (General.GetNullableInteger(ddlvessel.SelectedVessel.ToString()),//Request.QueryString["Vesselid"] == null ? ddlvessel.SelectedVessel.ToString() : Request.QueryString["Vesselid"].ToString()),
                null, null, null, sortexpression, sortdirection, Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvmedical.PageSize,
              ref iRowCount, ref iTotalPageCount, null, null, null, null, null, General.GetNullableString(txtRefNo.Text), General.GetNullableString(txtfileno.Text));


            gvmedical.DataSource = ds;
            gvmedical.VirtualItemCount = iRowCount;

         

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 
     protected void gvmedical_ItemCommand(object sender, GridCommandEventArgs e)
      {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = e.Item.ItemIndex;
                string Script = "";
                NameValueCollection nvc;

                if (Request.QueryString["mode"] == "custom")
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";

                    nvc = new NameValueCollection();
                    LinkButton lnkRefNumber = (LinkButton)e.Item.FindControl("lnkRefNumber");
                    nvc.Set(lnkRefNumber.ID, lnkRefNumber.Text.ToString());
                    RadLabel lblCrewName = (RadLabel)e.Item.FindControl("lblCrewName");
                    nvc.Set(lblCrewName.ID, lblCrewName.Text.ToString());
                    RadLabel lblMedicalId = (RadLabel)e.Item.FindControl("lblPNIMedicalCaseId");
                    nvc.Set(lblMedicalId.ID, lblMedicalId.Text.ToString());
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                    nvc = new NameValueCollection();
                    //nvc = Filter.CurrentPickListSelection;

                    LinkButton lnkRefNumber = (LinkButton)e.Item.FindControl("lnkRefNumber");
                    nvc.Set("lnkRefNumber", lnkRefNumber.Text.ToString());
                    RadLabel lblCrewName = (RadLabel)e.Item.FindControl("lblCrewName");
                    nvc.Set("lblCrewName", lblCrewName.Text.ToString());
                    RadLabel lblMedicalId = (RadLabel)e.Item.FindControl("lblPNIMedicalCaseId");
                    nvc.Set("lblPNIMedicalCaseId", lblMedicalId.Text.ToString());
                    RadLabel lblPNIMedicalCaseId = (RadLabel)e.Item.FindControl("lblPNIMedicalCaseId");

                    PhoenixAccountsPNIIntergeration.MedicalPandIPoMappingInsert(General.GetNullableGuid(Request.QueryString["orderid"].ToString())
                                                                                 , General.GetNullableGuid(lblPNIMedicalCaseId.Text));




                }

                Filter.CurrentPickListSelection = nvc;
                String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', '');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script,true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvmedical_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }


    protected void gvmedical_SortCommand(object sender, GridSortCommandEventArgs e)
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

   

  
  
  
}
