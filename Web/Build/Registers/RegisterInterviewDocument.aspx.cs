using System;
using System.Collections;
using System.Configuration;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.CrewOffshore;

public partial class Registers_RegisterInterviewDocument : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            //toolbar.AddFontAwesomeButton("../Registers/RegisterReportConfiguration.aspx", "Export to Excel","<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("../Registers/RegisterInterviewDocument.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvinterviewdocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersConfig.AccessRights = this.ViewState;
            MenuRegistersConfig.MenuList = toolbar.Show();

         
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["CURRENTINDEX"] = 1;
                gvinterviewdocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvinterviewdocument_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }
            CheckBoxList chkRankListEdit = (CheckBoxList)e.Item.FindControl("chkRankListEdit");
            if (chkRankListEdit != null)
            {
                chkRankListEdit.DataSource = PhoenixCrewOffshoreInterviewQuestion.ListRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                chkRankListEdit.DataTextField = "FLDRANKNAME";
                chkRankListEdit.DataValueField = "FLDRANKID";
                chkRankListEdit.DataBind();

                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankListEdit");
                foreach (ListItem li in chk.Items)
                {
                    string[] slist = dr["FLDRANKLIST"].ToString().Split(',');
                    foreach (string s in slist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }


            RadLabel lblrank1 = (RadLabel)e.Item.FindControl("lblRank");

         

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            RadComboBox DDLDOCUMENTADD = (RadComboBox)e.Item.FindControl("ddldocumentadd");
            if (DDLDOCUMENTADD != null)
            {
                DataTable dt = PhoenixRegisterInterviewDocumentCheckList.DocumentList(General.GetNullableInteger(ddldocumenttype.SelectedValue.ToString()));
                if (dt.Rows.Count > 0)
                {
                    DDLDOCUMENTADD.Items.Clear();
                    DDLDOCUMENTADD.DataSource = dt;
                    DDLDOCUMENTADD.DataTextField = "FLDDOCUMENTNAME";
                    DDLDOCUMENTADD.DataValueField = "FLDDOCUMENTID";
                    DDLDOCUMENTADD.DataBind();

                }
                DDLDOCUMENTADD.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            }
            CheckBoxList chkRankListAdd = (CheckBoxList)e.Item.FindControl("chkRankListAdd");
            if (chkRankListAdd != null)
            {
                chkRankListAdd.DataSource = PhoenixCrewOffshoreInterviewQuestion.ListRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                chkRankListAdd.DataTextField = "FLDRANKNAME";
                chkRankListAdd.DataValueField = "FLDRANKID";
                chkRankListAdd.DataBind();
            }
        }
    }

    protected void gvinterviewdocument_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadComboBox ddldocumentadd = (RadComboBox)e.Item.FindControl("ddldocumentadd");
                CheckBoxList chka = (CheckBoxList)e.Item.FindControl("chkRankListAdd");
                string RList = "";
                string RankList = "";
                foreach (ListItem li in chka.Items)
                {
                    if (li.Selected)
                    {
                        RList += li.Value + ",";
                    }
                }

                if (RList != "")
                {
                    RankList = "," + RList;
                }

                PhoenixRegisterInterviewDocumentCheckList.InterviewDocumentCheckListInsert(null
                                                                          , General.GetNullableInteger(ddldocumenttype.SelectedValue.ToString())
                                                                          , General.GetNullableInteger(ddldocumentadd.SelectedValue.ToString())
                                                                          , General.GetNullableString(RankList)
                                                                          );
                BindData();
                gvinterviewdocument.Rebind();
              
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;

                string configurationid = item.GetDataKeyValue("FLDINTERVIEWDOCCHECKLISTID").ToString();
                PhoenixRegisterInterviewDocumentCheckList.InterviewDocumentDelete(new Guid(configurationid));
                BindData();
                gvinterviewdocument.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                string configurationid = item.GetDataKeyValue("FLDINTERVIEWDOCCHECKLISTID").ToString();
                // DropDownList ddldocumentadd = (DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddldocumentedit");
             
                RadLabel lbldocumentidedit = (RadLabel)e.Item.FindControl("lbldocumentidedit");
             
                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkRankListEdit");
                string RList = "";
                string RankList = "";
                foreach (ListItem li in chk.Items)
                {
                    if (li.Selected)
                    {
                        RList += li.Value + ",";
                    }
                }

                if (RList != "")
                {
                    RankList = "," + RList;
                }
                PhoenixRegisterInterviewDocumentCheckList.InterviewDocumentCheckListInsert(General.GetNullableGuid(configurationid)
                                                                           , General.GetNullableInteger(ddldocumenttype.SelectedValue.ToString())
                                                                           , General.GetNullableInteger(lbldocumentidedit.Text)
                                                                           , General.GetNullableString(RankList)
                                                                           );
                BindData();
                gvinterviewdocument.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
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

    protected void gvinterviewdocument_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvinterviewdocument.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDOCUMENTNAME",  };
        string[] alCaptions = { "Document Name" };

        DataTable dt = PhoenixRegisterInterviewDocumentCheckList.InterviewDocumentSearch(General.GetNullableInteger(ddldocumenttype.SelectedValue.ToString())
                                                                                , (int)ViewState["PAGENUMBER"]
                                                                                , gvinterviewdocument.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvinterviewdocument", "Course", alCaptions, alColumns, ds);

        gvinterviewdocument.DataSource = ds;
        gvinterviewdocument.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void MenuRegistersConfig_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

         
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvinterviewdocument.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}