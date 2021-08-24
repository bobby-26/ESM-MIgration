using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Data;
using System.Collections.Specialized;
using System.Configuration;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenanceGlobalComponentTypeJob : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceGlobalComponentTypeJob.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        //toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPlannedMaintenanceJob')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlan.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceGlobalComponentTypeJob.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuJobs.AccessRights = this.ViewState;
        MenuJobs.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["COMPONENTTYPEID"] = "";
            gvPlannedMaintenanceJob.PageSize = General.ShowRecords(null);

            ViewState["FREQUENCY"] = "";
            ViewState["FREQUENCYTYPE"] = "";
            ViewState["CATEGORY"] = "";

            if (Request.QueryString["LAUNCHFROM"] != null && Request.QueryString["LAUNCHFROM"].ToString().ToUpper() == "MODEL")
            {
                NameValueCollection nvc = new NameValueCollection();
                if (Request.QueryString["COMPONENTNUMBER"] != null)
                {
                    if (nvc.Get("COMPONENTNUMBER") != null)
                        nvc.Remove("COMPONENTNUMBER");

                    nvc.Add("COMPONENTNUMBER", Request.QueryString["COMPONENTNUMBER"].ToString());
                }
                if (Request.QueryString["MODELID"] != null)
                {
                    ViewState["MODELID"] = Request.QueryString["MODELID"].ToString();
                }
                else
                    ViewState["MODELID"] = "";

                if (Request.QueryString["MODEL"] != null)
                {
                    if (nvc.Get("TYPE") != null)
                        nvc.Remove("TYPE");

                    nvc.Add("TYPE", Request.QueryString["MODEL"].ToString());
                }
                if (Request.QueryString["MAKE"] != null)
                {
                    if (nvc.Get("MAKE") != null)
                        nvc.Remove("MAKE");

                    nvc.Add("MAKE", Request.QueryString["MAKE"].ToString());
                }
                Filter.CurrentGlobalComponentTypeJobFilter = nvc;
            }
            else
            {
                ViewState["MODELID"] = "";
            }
            

        }
        
    }
    //protected void MenuComponent_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;



    //        if (CommandName.ToUpper().Equals("POPULATE"))
    //        {
    //            ucError.HeaderMessage = "Please provide the following required information";
    //            if (General.GetNullableInteger(ddlModel.SelectedValue) == null)
    //                ucError.ErrorMessage = "Model is required";
    //            if (General.GetNullableString(txtNumber.Text) == null)
    //                ucError.ErrorMessage = "Component Number is required";

    //            if (ucError.IsError)
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            PhoenixPlannedMaintenanceGlobalComponent.PopulateComponentType(int.Parse(ddlModel.SelectedValue), General.GetNullableString(txtNumber.Text));
    //            //txtNumber.Text = "";
    //        }
    //        else if (CommandName.ToUpper().Equals("SEARCH"))
    //        {
    //            ucError.HeaderMessage = "Please provide the following required information";
    //            //if (General.GetNullableInteger(ddlModel.SelectedValue) == null)
    //            //    ucError.ErrorMessage = "Model is required";

    //            //if (ucError.IsError)
    //            //{
    //            //    ucError.Visible = true;
    //            //    return;
    //            //}
    //            gvPlannedMaintenanceJob.CurrentPageIndex = 0;
    //            gvPlannedMaintenanceJob.Rebind();
                

    //        }
    //        else if (CommandName.ToUpper().Equals("VESSEL"))
    //        {
    //            ucError.HeaderMessage = "Please provide the following required information";
    //            if (General.GetNullableInteger(ddlModel.SelectedValue) == null)
    //                ucError.ErrorMessage = "Model is required";

    //            if (ucError.IsError)
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            string Script = "";
    //            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //            Script += " setTimeout(function(){ openNewWindow('Vessel', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentMigrate.aspx?ModelID=" + ddlModel.SelectedValue + "&ModelName=" + ddlModel.SelectedItem.Text + "')},500)";
    //            Script += "</script>" + "\n";



    //            RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void BindModel()
    //{
    //    int i1 = 0;
    //    int i2 = 0;
    //    ddlModel.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelSearch(null, General.GetNullableString(ddlMake.SelectedValue) != null ? General.GetNullableString(ddlMake.SelectedItem.Text) : null, General.GetNullableInteger(ddlStroke.SelectedValue), null, null, 1, 10000, ref i1, ref i2, General.GetNullableString(ddlCmpName.SelectedValue) != null ? General.GetNullableString(ddlCmpName.SelectedItem.Text) : null, General.GetNullableString(txtNumber.Text));
        
    //    ddlModel.DataBind();
    //    ddlModel.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    //}
    //protected void BindComponentList()
    //{
    //    ddlCmpName.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelComponentList(null,null);
    //    ddlCmpName.DataBind();
    //    ddlCmpName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    //}
    //protected void BindMake()
    //{
    //    ddlMake.DataSource = PhoenixPlannedMaintenanceGlobalComponent.RegisterModelMakeList(General.GetNullableString(ddlCmpName.SelectedValue) != null ? General.GetNullableString(ddlCmpName.SelectedItem.Text):null, General.GetNullableInteger(ddlStroke.SelectedValue));
    //    ddlMake.DataBind();
    //    ddlMake.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    //}


    
    protected void gvPlannedMaintenanceJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDCLASS", "FLDFREQUENCYNAME" };
            string[] alCaptions = { "Code", "Title", "Job Class", "Frequency" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            
            DataSet ds;
            NameValueCollection nvc = new NameValueCollection();
            if (Filter.CurrentGlobalComponentTypeJobFilter != null)
                nvc = Filter.CurrentGlobalComponentTypeJobFilter;
            int active = 1;
            if (nvc.Get("ACTIVE") != null)
            {
                active = nvc.Get("ACTIVE").ToString().ToUpper() == "NO" ? 0 : nvc.Get("ACTIVE").ToString().ToUpper() == "ALL" ? 2 : 1;
            }
            ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeJobFilterSearch(
                                                            General.GetNullableInteger(ViewState["MODELID"].ToString()),
                                                           nvc.Get("TYPE") !=null?General.GetNullableString(nvc.Get("TYPE").ToString()):null,
                                                           nvc.Get("MAKE") != null ? General.GetNullableString(nvc.Get("MAKE").ToString()) : null,
                                                           nvc.Get("COMPONENTNUMBER") != null ? General.GetNullableString(nvc.Get("COMPONENTNUMBER").ToString()) : null,
                                                           nvc.Get("COMPONENTNAME") != null ? General.GetNullableString(nvc.Get("COMPONENTNAME").ToString()) : null,
                                                           nvc.Get("CODE") != null ? General.GetNullableString(nvc.Get("CODE").ToString()) : null,
                                                           nvc.Get("TITLE") != null ? General.GetNullableString(nvc.Get("TITLE").ToString()) : null,
                                                           sortexpression, sortdirection,
                                                           gvPlannedMaintenanceJob.CurrentPageIndex + 1,
                                                           gvPlannedMaintenanceJob.PageSize, ref iRowCount, ref iTotalPageCount,active
                                                           ,nvc.Get("CATEGORY")!=null? General.GetNullableString(nvc.Get("CATEGORY").ToString()) : null
                                                           ,nvc.Get("FREQUENCY")!=null ? General.GetNullableInteger(nvc.Get("FREQUENCY").ToString()):null
                                                           , nvc.Get("FREQUENCYTYPE") != null ? General.GetNullableInteger(nvc.Get("FREQUENCYTYPE").ToString()) : null
                                                           , nvc.Get("COUNTERFREQUENCY") != null ? General.GetNullableInteger(nvc.Get("COUNTERFREQUENCY").ToString()) : null);

            gvPlannedMaintenanceJob.DataSource = ds;
            gvPlannedMaintenanceJob.VirtualItemCount = iRowCount;

            //if (Filter.CurrentGlobalComponentTypeJobFilter != null)
            //{
            //    foreach (GridColumn c in gvPlannedMaintenanceJob.MasterTableView.Columns)
            //    {
            //        c.CurrentFilterValue = nvc.Get(c.UniqueName) != null ? nvc.Get(c.UniqueName).ToString() : "";
            //    }
            //}

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPlannedMaintenanceJob_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)item.DataItem;
                LinkButton db = (LinkButton)e.Item.FindControl("cmdEdit");
                if (db != null)
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    //db.Attributes.Add("onclick", "javascript:return openNewWindow('Component', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentTypeJobEdit.aspx?GLOBALCOMPONENTTYPEMAPID=" + item.GetDataKeyValue("FLDGLOBALCOMPONENTTYPEJOBMAPID").ToString()+ "'); return false;");
                }
                RadLabel lblActive = (RadLabel)e.Item.FindControl("lblActive");
                if(lblActive!=null)
                {
                    lblActive.Text = drv["FLDAPPLICABLEYN"].ToString() == "1" ? "Yes" : "No";
                }

                RadLabel lblFrequency = (RadLabel)e.Item.FindControl("lblFrequency");
                if (lblFrequency != null)
                {
                    lblFrequency.Text = drv["FLDFREQUENCYNAME"].ToString() + "/" + drv["FLDCOUNTERFREQUENCYNAME"].ToString();
                }
                UserControlHard ucFrequencyType = (UserControlHard)e.Item.FindControl("ucFrequencyType");
                UserControlHard ucCounterType = (UserControlHard)e.Item.FindControl("ucCounterType");
                if (ucFrequencyType != null)
                {
                    ucFrequencyType.SelectedHard = drv["FLDFREQUENCYTYPE"].ToString();
                }
                if (ucCounterType != null)
                {
                    ucCounterType.SelectedHard = drv["FLDCOUNTERTYPE"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlannedMaintenanceJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if(e.CommandName == RadGrid.FilterCommandName)
            {
                GridFilteringItem filterItem = e.Item as GridFilteringItem;
                
                NameValueCollection nvc = new NameValueCollection();
                foreach(GridColumn c in gvPlannedMaintenanceJob.MasterTableView.Columns)
                {
                    if (filterItem[c.UniqueName].HasControls())
                    {
                        if(c.UniqueName !="CATEGORY" && c.UniqueName!="FREQUENCY")
                        {
                            TextBox filterBox = filterItem[c.UniqueName].Controls[0] as TextBox;
                            nvc.Remove(c.UniqueName);
                            nvc.Add(c.UniqueName, filterBox.Text);
                            
                               
                        }else if(c.UniqueName == "CATEGORY")
                        {
                            nvc.Remove(c.UniqueName);
                            nvc.Add(c.UniqueName, c.CurrentFilterValue);
                            ViewState["CATEGORY"] = c.CurrentFilterValue;
                        }else if(c.UniqueName == "FREQUENCY")
                        {
                            string freqfilter = c.CurrentFilterValue;
                            if (General.GetNullableString(freqfilter) != null)
                            {
                                string freq = freqfilter.Split('~')[0];
                                string freqtype = freqfilter.Split('~')[1];
                                if (freqtype != "-1")
                                {
                                    nvc["FREQUENCYTYPE"] = freqtype;
                                    nvc["FREQUENCY"] = freq;
                                }
                                else
                                {
                                    nvc["COUNTERFREQUENCY"] = freq;
                                }
                                ViewState["FREQUENCY"] = freq;
                                ViewState["FREQUENCYTYPE"] = freqtype;
                            }
                        }

                    }
                    
                }
                Filter.CurrentGlobalComponentTypeJobFilter = nvc;
                gvPlannedMaintenanceJob.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentJobDelete(new Guid(item.GetDataKeyValue("FLDGLOBALCOMPONENTJOBID").ToString()));
            }
            else if (e.CommandName.ToUpper().Equals("SELECT")|| e.CommandName.ToUpper().Equals("ROWCLICK"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " setTimeout(function(){ openNewWindow('Component', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentTypeJobEdit.aspx?GLOBALCOMPONENTTYPEMAPID=" + item.GetDataKeyValue("FLDGLOBALCOMPONENTTYPEJOBMAPID").ToString() + "&TABINDEX=0')},1000)";
                Script += "</script>" + "\n";



                RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
                //ClientScript.RegisterClientScriptBlock(typeof(Page), "bookmark", Script);
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
        try
        {
            gvPlannedMaintenanceJob.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 
    //protected void ddlCmpName_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    txtNumber.Text = ddlCmpName.SelectedValue;
    //    BindMake();
    //    BindModel();
        
    //}

    //protected void ddlStroke_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    BindMake();
    //    BindModel();
    //}

    //protected void ddlMake_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    BindModel();
    //}

    protected void ddlModel_SelectedIndexChanged1(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
    }

    protected void gvPlannedMaintenanceJob_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.CommandArgument.ToString();
        ViewState["SORTDIRECTION"] = ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0" ? "1" : "0";
        gvPlannedMaintenanceJob.Rebind();
    }

     
    protected void gvPlannedMaintenanceJob_PreRender(object sender, EventArgs e)
    {
       
        if(gvPlannedMaintenanceJob.MasterTableView.GetItems(GridItemType.FilteringItem)!=null)
        {
            GridFilteringItem item =(GridFilteringItem) gvPlannedMaintenanceJob.MasterTableView.GetItems(GridItemType.FilteringItem)[0];
            NameValueCollection nvc = new NameValueCollection();
            if (Filter.CurrentGlobalComponentTypeJobFilter != null)
                nvc = Filter.CurrentGlobalComponentTypeJobFilter;
            foreach (GridColumn c in gvPlannedMaintenanceJob.MasterTableView.Columns)
            {
                if (c.UniqueName != "FREQUENCY" && c.UniqueName != "CATEGORY")
                {
                    if (item[c.UniqueName].HasControls())
                    {
                        TextBox filterBox = item[c.UniqueName].Controls[0] as TextBox;
                        filterBox.Text = nvc.Get(c.UniqueName) != null ? nvc.Get(c.UniqueName).ToString() : "";
                        nvc.Remove(c.UniqueName);
                        nvc.Add(c.UniqueName, filterBox.Text);
                    }
                    c.CurrentFilterValue = nvc.Get(c.UniqueName) != null ? nvc.Get(c.UniqueName).ToString() : "";
                }



            }
        }
            
       
    }

    protected void gvcMenu_ItemClick(object sender, RadMenuEventArgs e)
    {
        int radGridClickedRowIndex;

        radGridClickedRowIndex = Convert.ToInt32(Request.Form["radGridClickedRowIndex"]);
        int index = 0;
        switch (e.Item.Value)
        {
            case "Edit":
                index = 0;
                break;
            case "History":
                index = 1;
                break;
            case "Include":
                index = 2;
                break;
            case "Manuals":
                index = 3;
                break;
        }

        GridDataItem item = gvPlannedMaintenanceJob.Items[radGridClickedRowIndex];
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        if(e.Item.Value == "Description")
        {
            Script += " setTimeout(function(){ openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?framename=ifMoreInfo&JOBID=" + item.GetDataKeyValue("FLDJOBID").ToString() + "','false','1066','320')},1000)";
        }
        else
        {
            Script += " setTimeout(function(){ openNewWindow('Component', '', 'PlannedMaintenance/PlannedMaintenanceGlobalComponentTypeJobEdit.aspx?GLOBALCOMPONENTTYPEMAPID=" + item.GetDataKeyValue("FLDGLOBALCOMPONENTTYPEJOBMAPID").ToString() + "&TABINDEX=" + index + "')},1000)";
        }
        
        Script += "</script>" + "\n";

        RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
    }

    protected void MenuJobs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentGlobalComponentTypeJobFilter = null;
                gvPlannedMaintenanceJob.Rebind();
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDMAKE", "FLDTYPE", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDCOUNTERFREQUENCYNAME", "FLDCATEGORY", "FLDAPPLICABLEYN" };
        string[] alCaptions = { "MAKE", "TYPE/MODEL", "NUMBER", "NAME", "CODE", "TITLE", "CALENDAR FREQUENCY", "COUNTER FREQUENCY", "JOB CATEGORY", "ACTIVE Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds;
        NameValueCollection nvc = new NameValueCollection();
        if (Filter.CurrentGlobalComponentTypeJobFilter != null)
            nvc = Filter.CurrentGlobalComponentTypeJobFilter;
        int active = 1;
        if (nvc.Get("ACTIVE") != null)
        {
            active = nvc.Get("ACTIVE").ToString().ToUpper() == "NO" ? 0 : nvc.Get("ACTIVE").ToString().ToUpper() == "ALL" ? 2 : 1;
        }
        ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeJobFilterSearch(
                                                        General.GetNullableInteger(ViewState["MODELID"].ToString()),
                                                       nvc.Get("TYPE") != null ? General.GetNullableString(nvc.Get("TYPE").ToString()) : null,
                                                       nvc.Get("MAKE") != null ? General.GetNullableString(nvc.Get("MAKE").ToString()) : null,
                                                       nvc.Get("COMPONENTNUMBER") != null ? General.GetNullableString(nvc.Get("COMPONENTNUMBER").ToString()) : null,
                                                       nvc.Get("COMPONENTNAME") != null ? General.GetNullableString(nvc.Get("COMPONENTNAME").ToString()) : null,
                                                       nvc.Get("CODE") != null ? General.GetNullableString(nvc.Get("CODE").ToString()) : null,
                                                       nvc.Get("TITLE") != null ? General.GetNullableString(nvc.Get("TITLE").ToString()) : null,
                                                       sortexpression, sortdirection,
                                                       1,
                                                       gvPlannedMaintenanceJob.VirtualItemCount, ref iRowCount, ref iTotalPageCount, active);

        DataTable dt = ds.Tables[0];
        //General.ShowExcel("COMPONENT TYPE JOBS", ds.Tables[0], alColumns, alCaptions, null, null);


        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=COMPONENT_TYPE_JOBS.xls");
        HttpContext.Current.Response.ContentType = "application/vnd.msexcel";
        HttpContext.Current.Response.Write("<style>.text{ mso-number-format:\"\\@\";}</style>");
        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        HttpContext.Current.Response.Write("<td ><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        HttpContext.Current.Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>&nbsp&nbsp&nbsp&nbsp COMPONENT TYPE JOBS</h3></td>");
        HttpContext.Current.Response.Write("</tr>");
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br />");
        HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            HttpContext.Current.Response.Write("<td width='20%'>");
            HttpContext.Current.Response.Write("<b>" + alCaptions[i] + "</b>");
            HttpContext.Current.Response.Write("</td>");
        }
        HttpContext.Current.Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            HttpContext.Current.Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(string)) ? "<td  class='text'>" : "<td>");
                if (alColumns[i] == "FLDAPPLICABLEYN")
                    HttpContext.Current.Response.Write(dr[alColumns[i]].ToString() == "1" ? "Yes" : "No");
                else
                    HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                HttpContext.Current.Response.Write("</td>");
            }
            HttpContext.Current.Response.Write("</tr>");
        }
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
        HttpContext.Current.Response.End();
    }
    protected void ddlJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void cblFrequencyType_DataBinding(object sender, EventArgs e)
    {
        RadComboBox frequency = sender as RadComboBox;
        frequency.DataSource = PhoenixRegistersHard.ListHard(1, 7);
        frequency.DataTextField = "FLDHARDNAME";
        frequency.DataValueField = "FLDHARDCODE";

        frequency.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        frequency.Items.Add(new RadComboBoxItem("Hours", "-1"));
    }
}