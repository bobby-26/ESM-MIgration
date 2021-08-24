using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
public partial class CrewPlanRelievee : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Crew/CrewPlanRelievee.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewPlanRelieveeFilter.aspx?Ispopup=" + Request.QueryString["Ispopup"] + "'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewPlanRelievee.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            QueryMenu.AccessRights = this.ViewState;
            QueryMenu.MenuList = toolbarsub.Show();


            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 0;
                ViewState["EMPID"] = "";
                ViewState["GROUPRANKID"] = "";
                ViewState["pl"] = "";
                ViewState["Ispopup"] = "";

                if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                    ViewState["pl"] = Request.QueryString["pl"].ToString();
                
                if (Request.QueryString["Ispopup"] != null && Request.QueryString["Ispopup"] != "")
                {
                    ViewState["Ispopup"] = Request.QueryString["Ispopup"].ToString();

                    if (Request.QueryString["RANKID"] != null)
                            ViewState["GROUPRANKID"] = Request.QueryString["RANKID"].ToString();

                    if (Request.QueryString["GroupRank"] != null)
                    {
                        ViewState["GROUPRANKID"] = Request.QueryString["GroupRank"].ToString();
                    }

                    Filter.CurrentPlanRelieveeFilterSelection = null;
                }

                gvSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }



            CreateTabs();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void CreateTabs()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Relief Plan", "RELIEFPLAN");
        toolbarmain.AddButton("Reliever", "RELIEVER");
        CrewRelieverTabs.AccessRights = this.ViewState;
        CrewRelieverTabs.MenuList = toolbarmain.Show();
        CrewRelieverTabs.SelectedMenuIndex = 0;
    }

    protected void CrewRelieverTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("RELIEVER"))
            {
                if (General.GetNullableInteger(ViewState["EMPID"].ToString()) == null)
                {
                    CrewRelieverTabs.SelectedMenuIndex = 0;
                    ucError.ErrorMessage = "Please select a Relievee";
                    ucError.Visible = true;
                    return;
                }
            }
            if (CommandName.ToUpper().Equals("RELIEVER"))
            {
                string strIsTop4 = "";

                GridDataItem item = gvSearch.Items[int.Parse(ViewState["CURRENTINDEX"].ToString())];
                strIsTop4 = ((RadLabel)item.FindControl("lblIsTop4")).Text;

                Response.Redirect("../Crew/CrewPlanRelieverNew.aspx?empid=" + ViewState["EMPID"] + "&vesselid=" + ViewState["VESSELID"] + "&rankid=" + ViewState["RANKID"] + "&reliefdate=" + ViewState["RELIEFDATE"] + "&selectedindex=" + ViewState["CURRENTINDEX"].ToString() + "&IsTop4=" + strIsTop4 + "&Ispopup" + ViewState["Ispopup"] + "&GroupRank" + ViewState["GROUPRANKID"], false);
            }
            else if (CommandName.ToUpper().Equals("RELIEFPLAN"))
            {
                Response.Redirect("../Crew/CrewPlanRelievee.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void QueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentPlanRelieveeFilterSelection = null;
                ViewState["PAGENUMBER"] = 1;
                gvSearch.CurrentPageIndex = 0;
                BindData();
                gvSearch.Rebind();
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

        string[] alColumns = { "FLDRANKNAME","FLDVESSELNAME", "FLDNAME","FLDOFFSIGNERZONE","FLDOFFSIGNERDECIMALEXPERIENCE","FLDRELIVEEVSLTYPEDECIMALEXPERIENCE", "FLDRELIEFDUEDATE","FLDEXPRIEDDATE", "FLDEXPECTEDJOINDATE",
                               "FLDRELIEVERNAME", "FLDRELIEVERRANK","FLDDATEOFREADINESS","FLDRELIEVERDECIMALEXPERIENCE", "FLDSEAPORTNAME","FLDCREWCHANGEREMARKS" , "FLDPDSTATUS"};
        string[] alCaptions = { "Rank","Vessel", "Off-Signer","Off-Signer Zone","Rank Exp.(M)","Vsl Typ exp", "Relief Due","Contract Expiry Date", "Planned Relief", "Reliever",
                                  "Reliever Rank","Readiness Date","Rank Exp.(M)","Planned Port","Crew Change Remarks","PD Status"  };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        byte? filteryn = 0;
        NameValueCollection nvc = Filter.CurrentPlanRelieveeFilterSelection;
        if (nvc != null && !string.IsNullOrEmpty(nvc.Get("ucVesselType")))
        {
            filteryn = (byte?)1;
        }
        DataTable dt =  PhoenixCrewPlanning.RelieveePlanQueryActivity(nvc != null ? nvc.Get("txtName") : string.Empty
                                                                  , nvc != null ? nvc.Get("ucRank") : string.Empty
                                                                  , nvc != null ? nvc.Get("ucVessel") : string.Empty
                                                                  , nvc != null ? nvc.Get("ucVesselType") : string.Empty
                                                                  , nvc != null ? nvc.Get("ucZone") : string.Empty
                                                                  , nvc != null ? nvc.Get("ucPool") : string.Empty
                                                                  , nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                  , null
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("txtReliefDue")) : null
                                                                  , sortexpression, sortdirection
                                                                  , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                  , iRowCount
                                                                  , ref iRowCount, ref iTotalPageCount
                                                                  , nvc != null ? nvc.Get("ucPrincipal") : string.Empty
                                                                  , Request.QueryString["access"] != null ? (byte?)General.GetNullableInteger("1") : null
                                                                  , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                  , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                  , filteryn
                                                                  , nvc != null ? nvc.Get("ucRankGroup") : string.Empty
                                                                  , (nvc != null ? General.GetNullableInteger(nvc["chkNotPlanned"]) : 1));


        General.ShowExcel("Relief Plan", dt, alColumns, alCaptions, null, sortexpression);
    }
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            byte? filteryn = 0;

            DataTable dt = new DataTable();

            NameValueCollection nvc = Filter.CurrentPlanRelieveeFilterSelection;
            if (Request.QueryString["pl"] != null)
            {
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    nvc.Add("txtReliefDue", string.Empty);
                    nvc.Add("txtName", string.Empty);
                    nvc.Add("ucRank", string.Empty);
                    nvc.Add("ucVessel", string.Empty);
                    nvc.Add("ucVesselType", string.Empty);
                    nvc.Add("ucPool", Request.QueryString["pl"]);//ucPool.SelectedPool);
                    nvc.Add("ucZone", string.Empty);
                    nvc.Add("lstNationality", string.Empty);//lstNationality.SelectedList);
                    nvc.Add("ucPrincipal", string.Empty);
                    nvc.Add("txtFromDate", string.Empty);
                    nvc.Add("txtToDate", string.Empty);
                    nvc.Add("ucRankGroup", string.Empty);
                    nvc.Add("chkNotPlanned", string.Empty);
                }
                else
                {
                    nvc["ucPool"] = Request.QueryString["pl"];
                }
            }

            if (nvc != null && !string.IsNullOrEmpty(nvc.Get("ucVesselType")))
            {
                filteryn = (byte?)1;
            }
            if (Request.QueryString["Ispopup"] != null) // from dashboard
            {
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    nvc.Add("txtReliefDue", "30");
                    nvc.Add("txtName", string.Empty);
                    nvc.Add("ucRank", string.Empty);
                    nvc.Add("ucVessel", string.Empty);
                    nvc.Add("ucVesselType", string.Empty);
                    nvc.Add("ucPool", string.Empty);//ucPool.SelectedPool);
                    nvc.Add("ucZone", string.Empty);
                    nvc.Add("lstNationality", string.Empty);//lstNationality.SelectedList);
                    nvc.Add("ucPrincipal", string.Empty);
                    nvc.Add("txtFromDate", string.Empty);
                    nvc.Add("txtToDate", string.Empty);
                    nvc.Add("ucRankGroup", ViewState["GROUPRANKID"].ToString());
                    nvc.Add("chkNotPlanned", "1");

                }
            }

            dt = PhoenixCrewPlanning.RelieveePlanQueryActivity(nvc != null ? nvc.Get("txtName") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucRank") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucVessel") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucVesselType") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucZone") : string.Empty
                                                                    , nvc != null ? nvc.Get("ucPool") : string.Empty
                                                                    , nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                    , null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtReliefDue")) : null
                                                                    , sortexpression, sortdirection
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvSearch.PageSize
                                                                    , ref iRowCount, ref iTotalPageCount
                                                                    , nvc != null ? nvc.Get("ucPrincipal") : string.Empty
                                                                    , Request.QueryString["access"] != null ? (byte?)General.GetNullableInteger("1") : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : null
                                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : null
                                                                    , filteryn
                                                                    , nvc != null ? nvc.Get("ucRankGroup") : string.Empty
                                                                    , (nvc != null ? General.GetNullableInteger(nvc["chkNotPlanned"]) : 1));


            gvSearch.DataSource = dt;
            gvSearch.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSearch.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;

            int selectedRowIndex = dataItem.RowIndex;

        }

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblCrewPlanId = (RadLabel)e.Item.FindControl("lblCrewPlanId");
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");

            RadLabel lblRelieveeId = (RadLabel)e.Item.FindControl("lblEmployeeId");
            RadLabel lblRelieverId = (RadLabel)e.Item.FindControl("lblRelieverId");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkRelievee");
            RadLabel VesselId = (RadLabel)e.Item.FindControl("lblVesselId");

            if (lbr != null)
            {
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblRelieveeId.Text + "'); return false;");
            }

            LinkButton lbrv = (LinkButton)e.Item.FindControl("lnkReliever");
            if (lbrv != null)
            {
                lbrv.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblRelieverId.Text + "'); return false;");
            }

            if (eb != null)
            {
                eb.Attributes.Add("onclick", "javascript:openNewWindow('Edit','','" + Session["sitepath"] + "/Crew/CrewPlanRelieveeEdit.aspx?empid=" + lblRelieveeId.Text + "&crewplanid=" + lblCrewPlanId.Text + "&vesselid=" + VesselId.Text + "'); return true;");
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }

            if (drv["FLDRELIEFDUEDATE"].ToString() != "")
            {
                if (Convert.ToDateTime(drv["FLDRELIEFDUEDATE"]) < DateTime.Now.AddDays(-1))
                {
                    RadLabel reliefduedate = (RadLabel)e.Item.FindControl("lblReliefDue");
                    reliefduedate.ForeColor = System.Drawing.Color.Red;
                }
            }

            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;

            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to deplan the seafarer ?')");
                if (lblCrewPlanId.Text == string.Empty)
                {
                    db.Visible = false;
                    eb.Visible = false;

                    imgFlag.Visible = false;
                }
                else
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/blue.png";
                }
            }

            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }

            if (drv["FLDCREWPLANID"].ToString() == "")
            {
                eb.Visible = false;
            }

            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");
            RadLabel relieverid = (RadLabel)e.Item.FindControl("lblRelieverId");
            RadLabel rank = (RadLabel)e.Item.FindControl("lblRankId");
            RadLabel joindate = (RadLabel)e.Item.FindControl("lblJoinDate");
            RadLabel aptype = (RadLabel)e.Item.FindControl("lblAppType");
            RadLabel lblSupt = (RadLabel)e.Item.FindControl("lblSupt");
            RadLabel lblpdstatus = (RadLabel)e.Item.FindControl("lblPDStatus");
            RadLabel lblRelieverRankId = (RadLabel)e.Item.FindControl("lblRelieverRankId");

            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
            LinkButton ckl = (LinkButton)e.Item.FindControl("cmdChkList");
            LinkButton sho = (LinkButton)e.Item.FindControl("cmdShow");

            if (pd != null)
            {
                pd.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                pd.Attributes.Add("onclick", "parent.openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid=" + relieverid.Text + "&vslid=" + VesselId.Text + "&rankid=" + rank.Text + "');return false;");
            }
            if (ckl != null)
            {
                ckl.Visible = SessionUtil.CanAccess(this.ViewState, ckl.CommandName);
                ckl.Attributes.Add("onclick", "parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CHECKLIST&employeeid=" + relieverid.Text + "&vesselid=" + VesselId.Text + "&joindate=" + joindate.Text + "&rankid=" + rank.Text + "&planid=" + lblCrewPlanId.Text + "'); return false;");
            }
            if (sho != null)
            {
                sho.Visible = SessionUtil.CanAccess(this.ViewState, sho.CommandName);
            }

            if (!string.IsNullOrEmpty(relieverid.Text))
            {
                pd.Visible = true;
                ckl.Visible = true;
            }
            else
            {
                pd.Visible = false;
                ckl.Visible = false;
            }


            if (empid.Text == string.Empty) sho.Visible = false;
            if (drv["FLDFAMILYNAME"].ToString() != string.Empty)
            {
                sho.Visible = false;
                ((RadLabel)e.Item.FindControl("lblName")).Text = drv["FLDFAMILYNAME"].ToString();
            }

            if (lblCrewPlanId.Text == "")
            {
                if (db != null)
                    db.Visible = false;
            }

            UserControlCommonToolTip ttip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            RadLabel lblsignonoffid = (RadLabel)e.Item.FindControl("lblsignonoffid");
            if (ttip != null)
            {
                ttip.Screen = "Crew/CrewPlanRelieveeInfo.aspx?signonoffid=" + lblsignonoffid.Text + "&crewplanid=" + lblCrewPlanId.Text;
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPDStatus");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }

        }
    }


    protected void gvSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;

            if (e.CommandName.ToUpper() == "SHOW")
            {
                ViewState["RPAGENUMBER"] = 1;

                ViewState["CURRENTINDEX"] = e.Item.ItemIndex.ToString();

                string selectedRowIndex = ViewState["CURRENTINDEX"].ToString();

                string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                string relieverid = ((RadLabel)e.Item.FindControl("lblRelieverId")).Text;
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string rankid = ((RadLabel)e.Item.FindControl("lblRankid")).Text;
                string reliefdate = ((RadLabel)e.Item.FindControl("lblReliefDue")).Text;
                string RelieverRankId = ((RadLabel)e.Item.FindControl("lblRelieverRankId")).Text;
                string strIsTop4 = ((RadLabel)e.Item.FindControl("lblIsTop4")).Text;
                string vsltype = ((RadLabel)e.Item.FindControl("lblVesseltype")).Text;

                ViewState["EMPID"] = empid;
                ViewState["VESSELID"] = vesselid;
                ViewState["RANKID"] = rankid;
                ViewState["RELIEFDATE"] = reliefdate;
                ViewState["VSLTYPE"] = vsltype;

                Response.Redirect("CrewPlanRelieverNew.aspx?empid=" + ViewState["EMPID"] + "&rlvrankid=" + RelieverRankId + "&vesselid=" + ViewState["VESSELID"] + "&rankid=" + ViewState["RANKID"] + "&reliefdate=" + ViewState["RELIEFDATE"] + "&selectedindex=" + selectedRowIndex + "&relieverid=" + relieverid + "&IsTop4=" + strIsTop4 + "&Ispopup=" + ViewState["Ispopup"] + "&GroupRank=" + ViewState["GROUPRANKID"], false);
            }

            if (e.CommandName.ToUpper().Equals("SELECTROW"))
            {

                gvSearch.SelectedIndexes.Clear();

                gvSearch.SelectedIndexes.Add(e.Item.ItemIndex);

                ViewState["CURRENTINDEX"] = e.Item.ItemIndex;

                string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string rankid = ((RadLabel)e.Item.FindControl("lblRankid")).Text;
                string reliefdate = ((RadLabel)e.Item.FindControl("lblReliefDue")).Text;

                ViewState["EMPID"] = empid;
                ViewState["VESSELID"] = vesselid;
                ViewState["RANKID"] = rankid;
                ViewState["RELIEFDATE"] = reliefdate;
            }

            if (e.CommandName.ToUpper() == "ROWCLICK" || e.CommandName.ToUpper() == "SELECT")
            {
                gvSearch.SelectedIndexes.Clear();

                gvSearch.SelectedIndexes.Add(e.Item.ItemIndex);

                ViewState["CURRENTINDEX"] = e.Item.ItemIndex;

                string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string rankid = ((RadLabel)e.Item.FindControl("lblRankid")).Text;
                string reliefdate = ((RadLabel)e.Item.FindControl("lblReliefDue")).Text;

                ViewState["EMPID"] = empid;
                ViewState["VESSELID"] = vesselid;
                ViewState["RANKID"] = rankid;
                ViewState["RELIEFDATE"] = reliefdate;

            }
            if (e.CommandName == "Page")
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


    protected void gvSearch_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string crewPlanId = ((RadLabel)e.Item.FindControl("lblCrewPlanId")).Text;
            string empid = ((RadLabel)e.Item.FindControl("lblRelieverId")).Text;
            PhoenixCrewPlanning.DeleteCrewPlan(new Guid(crewPlanId), int.Parse(empid));
            BindData();
            gvSearch.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvSearch_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvSearch.Rebind();
    }




}
