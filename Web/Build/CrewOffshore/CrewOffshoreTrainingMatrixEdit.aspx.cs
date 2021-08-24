using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;

public partial class CrewOffshoreTrainingMatrixEdit : PhoenixBasePage
{
    public string strCoc, strStcw, strOtherDoc, strMedicalDoc;
   

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ucManager.AddressType = ((int)PhoenixAddressType.MANAGER).ToString();
            ucCharterer.AddressType = ((int)PhoenixAddressType.CHARTERER).ToString();

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarsub.AddButton("Details", "DETAILS", ToolBarDirection.Right);
            toolbarsub.AddButton("List", "LIST",ToolBarDirection.Right);
            
            
            CrewTrainingMenu.AccessRights = this.ViewState;
            CrewTrainingMenu.MenuList = toolbarsub.Show();
            if (!IsPostBack)
            {
                //chkRank.Checked = true;
                //chkVesselType.Checked = true;
                //chkCharterer.Checked = true;
                ViewState["MATRIXID"] = "";
                ViewState["vsltype"] = "";
                ViewState["rank"] = "";
                ViewState["charterer"] = "";
                if (Request.QueryString["matrixid"] != null && Request.QueryString["matrixid"].ToString() != "")
                {
                    ViewState["MATRIXID"] = Request.QueryString["matrixid"].ToString();
                }
                if (!String.IsNullOrEmpty(ViewState["MATRIXID"].ToString()))
                    BindFields();
                else
                {
                    //BindCOC(lstCOC, 2, null);
                    //BindCourse(lstCourse, 2, null);
                    //BindOtherDocs(lstOtherDocs, 2, null);
                    //BindUserDefinedDocs(lstUserDefinedList, 2, null);
                    BindRank(lstExpinRank, 2, null);
                    BindVesselType(lstExpinVesseltype, 2, null);
                }
            }
           // BindDocumentCategory();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindFields()
    {
        DataTable dt = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixEdit(int.Parse(ViewState["MATRIXID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtMatrixName.Text = dr["FLDMATRIXNAME"].ToString();
            ucVesselType.SelectedVesseltype = dr["FLDVESSELTYPEID"].ToString();
            ViewState["vsltype"] = dr["FLDVESSELTYPEID"].ToString();
            ucCharterer.SelectedAddress = dr["FLDCHARTERERID"].ToString();
            ViewState["charterer"] = dr["FLDCHARTERERID"].ToString();
            ucRank.SelectedRank = dr["FLDRANKID"].ToString();
            ViewState["rank"] = dr["FLDRANKID"].ToString();
            ucManager.SelectedAddress = dr["FLDMANAGEMENTCOMPANYID"].ToString();

            //if (dr["FLDVESSELTYPEID"].ToString() == null || dr["FLDVESSELTYPEID"].ToString() == "")
            //    chkVesselType.Checked = false;
            //if (dr["FLDCHARTERERID"].ToString() == null || dr["FLDCHARTERERID"].ToString() == "")
            //    chkCharterer.Checked = false;
            //if (dr["FLDRANKID"].ToString() == null || dr["FLDRANKID"].ToString() == "")
            //    chkRank.Checked = false;

            //BindCOC(lstCOC, 2, dr["FLDCOCLIST"].ToString());
            //BindCOC(lstSelectedCOC, 1, dr["FLDCOCLIST"].ToString());
            //BindCourse(lstCourse, 2, dr["FLDSTCWLIST"].ToString());
            //BindCourse(lstSelectedCourse, 1, dr["FLDSTCWLIST"].ToString());
            //BindOtherDocs(lstOtherDocs, 2, dr["FLDOTHERDOCLIST"].ToString());
            //BindOtherDocs(lstSelectedOtherDocs, 1, dr["FLDOTHERDOCLIST"].ToString());
            //BindUserDefinedDocs(lstUserDefinedList, 2, dr["FLDUSERDEFINEDDOCLIST"].ToString());
            //BindUserDefinedDocs(lstSelectedUserDefinedList, 1, dr["FLDUSERDEFINEDDOCLIST"].ToString());
            BindRank(lstExpinRank, 2, dr["FLDEXPERIENCEINRANKLIST"].ToString());
            BindRank(lstSelectedExpinRank, 1, dr["FLDEXPERIENCEINRANKLIST"].ToString());
            txtRankExp.Text = dr["FLDRANKEXPERIENCE"].ToString();
            BindVesselType(lstExpinVesseltype, 2, dr["FLDEXPERIENCEVESSELTYPELIST"].ToString());
            BindVesselType(lstSelectedExpinVesseltype, 1, dr["FLDEXPERIENCEVESSELTYPELIST"].ToString());
            txtVesseltypeExp.Text = dr["FLDVESSELTYPEEXPERIENCE"].ToString();

            //BindCOCtoWaive(cblWaivedCOC, 1, dr["FLDCOCLIST"].ToString());
            //BindCoursetoWaive(cblWaivedCourse, 1, dr["FLDSTCWLIST"].ToString());
            //BindOtherDocstoWaive(cblWaivedOtherdocs, 1, dr["FLDOTHERDOCLIST"].ToString());
            //BindUserDefinedDocstoWaive(cblWaivedUserdoc, 1, dr["FLDUSERDEFINEDDOCLIST"].ToString());

            //SetSelectedItemsForCheckBoxList(cblWaivedCOC, dr["FLDWAIVEDCOCLIST"].ToString());
            //SetSelectedItemsForCheckBoxList(cblWaivedCourse, dr["FLDWAIVEDSTCWLIST"].ToString());
            //SetSelectedItemsForCheckBoxList(cblWaivedOtherdocs, dr["FLDWAIVEDOTHERDOCLIST"].ToString());
            //SetSelectedItemsForCheckBoxList(cblWaivedUserdoc, dr["FLDWAIVEDUSERDOCLIST"].ToString());

            if (dr["FLDEXPERIENCEINRANKLIST"].ToString() != null && dr["FLDEXPERIENCEINRANKLIST"].ToString() != "")
                chkWaivedRankExp.Enabled = true;

            if (dr["FLDEXPERIENCEVESSELTYPELIST"].ToString() != null && dr["FLDEXPERIENCEVESSELTYPELIST"].ToString() != "")
                chkWaivedVesselTypeExp.Enabled = true;

            if (dr["FLDRANKEXPWAIVEYN"].ToString() == "1")
                chkWaivedRankExp.Checked = true;

            if (dr["FLDVESSELTYPEEXPWAIVEYN"].ToString() == "1")
                chkWaivedVesselTypeExp.Checked = true;

        }
    }

    protected void setFilter(object sender, EventArgs e)
    {
        ViewState["vsltype"] = ucVesselType.SelectedVesseltype;
        ViewState["rank"] = ucRank.SelectedRank;
        ViewState["charterer"] = ucCharterer.SelectedAddress;
        BindDocumentCategory();
    }

    protected void btnExpinRankSelect_Click(object sender, EventArgs e)
    {
        string sellistfromavailable = GetSelectedItems(lstExpinRank);
        string list = GetAllItems(lstSelectedExpinRank);
        list = list + "," + sellistfromavailable;
        BindRank(lstSelectedExpinRank, 1, list);
        BindRank(lstExpinRank, 2, list);
    }

    protected void btnExpinRankDeselect_Click(object sender, EventArgs e)
    {
        string sellistfromselected = GetSelectedItems(lstSelectedExpinRank);
        string list = GetAllItems(lstExpinRank);
        list = list + "," + sellistfromselected;
        BindRank(lstExpinRank, 1, list);
        BindRank(lstSelectedExpinRank, 2, list);
    }

    protected void btnExpinVTSelect_Click(object sender, EventArgs e)
    {
        string sellistfromavailable = GetSelectedItems(lstExpinVesseltype);
        string list = GetAllItems(lstSelectedExpinVesseltype);
        list = list + "," + sellistfromavailable;
        BindVesselType(lstSelectedExpinVesseltype, 1, list);
        BindVesselType(lstExpinVesseltype, 2, list);
    }

    protected void btnExpinVTDeselect_Click(object sender, EventArgs e)
    {
        string sellistfromselected = GetSelectedItems(lstSelectedExpinVesseltype);
        string list = GetAllItems(lstExpinVesseltype);
        list = list + "," + sellistfromselected;
        BindVesselType(lstExpinVesseltype, 1, list);
        BindVesselType(lstSelectedExpinVesseltype, 2, list);
    }

    protected void BindVesselType(ListBox lst, int? includeexclude, string list)
    {
        lst.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixVesselTypeList(includeexclude, General.GetNullableString(list));
        lst.DataTextField = "FLDTYPEDESCRIPTION";
        lst.DataValueField = "FLDVESSELTYPEID";
        lst.DataBind();
    }

    protected void BindRank(ListBox lst, int? includeexclude, string list)
    {
        lst.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixRankList(includeexclude, General.GetNullableString(list));
        lst.DataTextField = "FLDRANKNAME";
        lst.DataValueField = "FLDRANKID";
        lst.DataBind();
    }

    protected void BindCOC(ListBox lst, int? includeexclude, string list)
    {
        lst.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixDocumentLicenceList(null, null, 1, null, null,
                                                                includeexclude, General.GetNullableString(list));
        lst.DataTextField = "FLDDOCUMENTNAME";
        lst.DataValueField = "FLDDOCUMENTID";
        lst.DataBind();
    }

    protected void BindCOCtoWaive(CheckBoxList cbl, int? includeexclude, string list)
    {
        cbl.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixDocumentLicenceList(null, null, 1, null, null,
                                                                includeexclude, General.GetNullableString(list), 1);
        cbl.DataTextField = "FLDDOCUMENTNAME";
        cbl.DataValueField = "FLDDOCUMENTID";
        cbl.DataBind();

    }

    protected void BindCourse(ListBox lst, int? includeexclude, string list)
    {
        string doctype = PhoenixCommonRegisters.GetHardCode(1, 103, "6"); // STCW
        lst.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixDocumentCourseList(General.GetNullableInteger(doctype),
                                                includeexclude, General.GetNullableString(list));
        lst.DataTextField = "FLDDOCUMENTNAME";
        lst.DataValueField = "FLDDOCUMENTID";
        lst.DataBind();
    }

    protected void BindCoursetoWaive(CheckBoxList cbl, int? includeexclude, string list)
    {
        string doctype = PhoenixCommonRegisters.GetHardCode(1, 103, "6"); // STCW
        cbl.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixDocumentCourseList(General.GetNullableInteger(doctype),
                                                includeexclude, General.GetNullableString(list), 1);
        cbl.DataTextField = "FLDDOCUMENTNAME";
        cbl.DataValueField = "FLDDOCUMENTID";
        cbl.DataBind();
    }

    protected void BindOtherDocs(ListBox lst, int? includeexclude, string list)
    {
        lst.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixDocumentOtherList(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString(), includeexclude, General.GetNullableString(list));
        lst.DataTextField = "FLDDOCUMENTNAME";
        lst.DataValueField = "FLDDOCUMENTID";
        lst.DataBind();
    }

    protected void BindOtherDocstoWaive(CheckBoxList cbl, int? includeexclude, string list)
    {
        cbl.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixDocumentOtherList(((int)PhoenixDocumentType.OTHER_DOC_NONE).ToString(), includeexclude, General.GetNullableString(list), 1);
        cbl.DataTextField = "FLDDOCUMENTNAME";
        cbl.DataValueField = "FLDDOCUMENTID";
        cbl.DataBind();
    }

    protected void BindUserDefinedDocs(ListBox lst, int? includeexclude, string list)
    {
        lst.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixUserDefinedDocsList(1, 109, includeexclude, General.GetNullableString(list));
        lst.DataTextField = "FLDQUICKNAME";
        lst.DataValueField = "FLDQUICKCODE";
        lst.DataBind();
    }

    protected void BindUserDefinedDocstoWaive(CheckBoxList cbl, int? includeexclude, string list)
    {
        cbl.DataSource = PhoenixCrewOffshoreTrainingMatrix.TrainingMatrixUserDefinedDocsList(1, 109, includeexclude, General.GetNullableString(list), 1);
        cbl.DataTextField = "FLDQUICKNAME";
        cbl.DataValueField = "FLDQUICKCODE";
        cbl.DataBind();
    }

    protected void CrewTrainingMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingMatrixList.aspx");
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreTrainingMatrixDetails.aspx?matrixid=" + ViewState["MATRIXID"].ToString());
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveMatrix();
                BindDocumentCategory();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SaveMatrix()
    {
        if (!IsValidMatrix())
        {
            ucError.Visible = true;
            return;
        }

        DocumentList();

        if (ViewState["MATRIXID"].ToString() == null || ViewState["MATRIXID"].ToString() == "")
        {
            int? matrixid = null;
            PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixInsert(int.Parse(ucVesselType.SelectedVesseltype)
            , int.Parse(ucCharterer.SelectedAddress)
            , int.Parse(ucRank.SelectedRank)
            , int.Parse(ucManager.SelectedAddress)
            , General.GetNullableString(strCoc)
            , General.GetNullableString(strStcw)
            , General.GetNullableString(strOtherDoc)
            , null
            , General.GetNullableString(GetAllItems(lstSelectedExpinRank))
            , General.GetNullableInteger(txtRankExp.Text)
            , General.GetNullableString(GetAllItems(lstSelectedExpinVesseltype))
            , General.GetNullableInteger(txtVesseltypeExp.Text)
            , null
            , null
            , null
            , null
            , ref matrixid
            , (chkWaivedRankExp.Checked) ? 1 : 0
            , (chkWaivedVesselTypeExp.Checked) ? 1 : 0
            , General.GetNullableString(strMedicalDoc)
            );

            ViewState["MATRIXID"] = matrixid;
        }
        else
        {
            PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixUpdate(int.Parse(ViewState["MATRIXID"].ToString())
                , int.Parse(ucVesselType.SelectedVesseltype)
                , int.Parse(ucCharterer.SelectedAddress)
                , int.Parse(ucRank.SelectedRank)
                , int.Parse(ucManager.SelectedAddress)
                , General.GetNullableString(strCoc)
                , General.GetNullableString(strStcw)
                , General.GetNullableString(strOtherDoc)
                , null
                , General.GetNullableString(GetAllItems(lstSelectedExpinRank))
                , General.GetNullableInteger(txtRankExp.Text)
                , General.GetNullableString(GetAllItems(lstSelectedExpinVesseltype))
                , General.GetNullableInteger(txtVesseltypeExp.Text)
                , null
                , null
                , null
                , null
                , (chkWaivedRankExp.Checked) ? 1 : 0
                , (chkWaivedVesselTypeExp.Checked) ? 1 : 0
                , General.GetNullableString(strMedicalDoc)
                );
        }

        ucStatus.Text = "Information updated.";
        BindFields();
    }

    private bool IsValidMatrix()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVesselType.SelectedVesseltype) == null)
            ucError.ErrorMessage = "Vessel Type is required.";

        if (General.GetNullableInteger(ucRank.SelectedRank) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableInteger(ucCharterer.SelectedAddress) == null)
            ucError.ErrorMessage = "Charterer is required.";

        if (General.GetNullableInteger(ucManager.SelectedAddress) == null)
            ucError.ErrorMessage = "Management company is required.";

        return (!ucError.IsError);
    }

    protected string GetAllItems(ListBox lst)
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in lst.Items)
        {
            strlist.Append(item.Value.ToString());
            strlist.Append(",");
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    protected string GetSelectedItems(ListBox lst)
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in lst.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    protected string GetSelectedItemsForCheckBoxList(CheckBoxList clst)
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in clst.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }
    protected void SetSelectedItemsForCheckBoxList(CheckBoxList clst, string list)
    {
        string[] waivelist;
        waivelist = list.Split(',');

        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in clst.Items)
        {
            foreach (string waive in waivelist)
            {
                if (item.Value == waive)
                {
                    item.Selected = true;
                }
            }

        }

    }



    private void BindDocumentCategory()
    {
        DataSet ds = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixDocumentCategoryList(General.GetNullableInteger(ViewState["MATRIXID"].ToString()));
        gvDocumentCategory.DataSource = ds;
   
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

    private void DocumentList()
    {
        StringBuilder strListcoc = new StringBuilder();
        StringBuilder strListstcw = new StringBuilder();
        StringBuilder strListotherdoc = new StringBuilder();
        StringBuilder strListMedical = new StringBuilder();
        foreach (GridDataItem gvr in gvDocumentCategory.Items)
        {
            if (gvDocumentCategory.Items.Count > 0)
            {
                CheckBoxList Listcoc = (CheckBoxList)gvr.FindControl("chkListcoc");
                foreach (ListItem li in Listcoc.Items)
                {
                    if (li.Selected == true)
                    {
                        strListcoc.Append("," + li.Value + ",");
                    }
                }

                CheckBoxList Liststcw = (CheckBoxList)gvr.FindControl("chkListstcw");
                foreach (ListItem li in Liststcw.Items)
                {
                    if (li.Selected == true)
                    {
                        strListstcw.Append("," + li.Value + ",");
                    }
                }

                CheckBoxList Listotherdoc = (CheckBoxList)gvr.FindControl("chkListotherdoc");
                foreach (ListItem li in Listotherdoc.Items)
                {
                    if (li.Selected == true)
                    {
                        strListotherdoc.Append("," + li.Value + ",");
                    }
                }

                CheckBoxList ListMedicalDoc = (CheckBoxList)gvr.FindControl("chkListMedical");
                foreach (ListItem li in ListMedicalDoc.Items)
                {
                    if (li.Selected == true)
                    {
                        strListMedical.Append("," + li.Value + ",");
                    }
                }
            }
        }

        strCoc = strListcoc.ToString().Replace(",,", ",");
        strStcw = strListstcw.ToString().Replace(",,", ",");
        strOtherDoc = strListotherdoc.ToString().Replace(",,", ",");
        strMedicalDoc = strListMedical.ToString().Replace(",,", ",");
    }

    protected void gvDocumentCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindDocumentCategory();
    }

    protected void gvDocumentCategory_ItemDataBound1(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {

                string RankList = "", VesselTypeList = "", ChartererList = "";
                if (General.GetNullableInteger(ViewState["rank"].ToString()) != null)
                    RankList = "," + ViewState["rank"].ToString() + ",";
                if (General.GetNullableInteger(ViewState["vsltype"].ToString()) != null)
                    VesselTypeList = "," + ViewState["vsltype"].ToString() + ",";
                if (General.GetNullableInteger(ViewState["charterer"].ToString()) != null)
                    ChartererList = "," + ViewState["charterer"].ToString() + ",";

                DataSet ds = new DataSet();
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (chkshowall.Checked == true)
                    ds = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixCategoryDocumentList(General.GetNullableInteger(drv["FLDDOCUMENTCATEGORYID"].ToString()), 1, General.GetNullableInteger(ViewState["MATRIXID"].ToString())
                    , null, null, null);
                else
                    ds = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixCategoryDocumentList(General.GetNullableInteger(drv["FLDDOCUMENTCATEGORYID"].ToString()), 1, General.GetNullableInteger(ViewState["MATRIXID"].ToString())
                    , General.GetNullableString(RankList)
                    , General.GetNullableString(VesselTypeList)
                    , General.GetNullableString(ChartererList));
                CheckBoxList cocList = (CheckBoxList)e.Item.FindControl("chkListcoc");
                DataTable dt = ds.Tables[0];
                if (cocList != null)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (dt.Select("FLDDOCUMENTTYPE = 1").Length > 0)
                        {
                            DataRow[] dr = dt.Select("FLDDOCUMENTTYPE = 1");

                            DataTable result = dr.CopyToDataTable();
                            if (result.Rows.Count > 0)
                            {
                                cocList.DataSource = result;
                                cocList.DataTextField = "FLDDOCUMENTNAME";
                                cocList.DataValueField = "FLDDOCUMENTID";
                                cocList.DataBind();
                                int i;
                                i = 0;
                                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chkListcoc");
                                foreach (ListItem li in chk.Items)
                                {
                                    if (i <= result.Rows.Count - 1)
                                    {
                                        string slist = result.Rows[i]["FLDSELECTTEDDOCUMENTID"].ToString();

                                        if (li.Value.Equals(slist))
                                        {
                                            li.Selected = true;
                                        }
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
                CheckBoxList stcwList = (CheckBoxList)e.Item.FindControl("chkListstcw");
                if (stcwList != null)
                {

                    DataRow[] dr2 = dt.Select("FLDDOCUMENTTYPE = 2");
                    if (dt.Select("FLDDOCUMENTTYPE = 2").Length > 0)
                    {
                        DataTable result2 = dr2.CopyToDataTable();

                        if (result2.Rows.Count > 0)
                        {

                            stcwList.DataSource = result2;
                            stcwList.DataTextField = "FLDDOCUMENTNAME";
                            stcwList.DataValueField = "FLDDOCUMENTID";
                            stcwList.DataBind();
                            int j;
                            j = 0;

                            CheckBoxList chk2 = (CheckBoxList)e.Item.FindControl("chkListstcw");
                            foreach (ListItem li in chk2.Items)
                            {
                                if (j <= result2.Rows.Count - 1)
                                {
                                    string slist = result2.Rows[j]["FLDSELECTTEDDOCUMENTID"].ToString();
                                    if (li.Value.Equals(slist))
                                    {
                                        li.Selected = true;
                                    }
                                    j++;
                                }
                            }
                        }
                    }
                }
                CheckBoxList chkListotherdoc = (CheckBoxList)e.Item.FindControl("chkListotherdoc");
                if (chkListotherdoc != null)
                {
                    DataRow[] dr3 = dt.Select("FLDDOCUMENTTYPE = 3");
                    if (dt.Select("FLDDOCUMENTTYPE = 3").Length > 0)
                    {
                        DataTable result2 = dr3.CopyToDataTable();

                        if (result2.Rows.Count > 0)
                        {

                            chkListotherdoc.DataSource = result2;
                            chkListotherdoc.DataTextField = "FLDDOCUMENTNAME";
                            chkListotherdoc.DataValueField = "FLDDOCUMENTID";
                            chkListotherdoc.DataBind();
                            int j;
                            j = 0;
                            CheckBoxList chk2 = (CheckBoxList)e.Item.FindControl("chkListotherdoc");
                            foreach (ListItem li in chk2.Items)
                            {
                                if (j <= result2.Rows.Count - 1)
                                {
                                    string slist = result2.Rows[j]["FLDSELECTTEDDOCUMENTID"].ToString();
                                    if (li.Value.Equals(slist))
                                    {
                                        li.Selected = true;
                                    }
                                    j++;
                                }
                            }
                        }

                    }
                }

                CheckBoxList chkListMedical = (CheckBoxList)e.Item.FindControl("chkListMedical");
                if (chkListMedical != null)
                {
                    DataRow[] dr4 = dt.Select("FLDDOCUMENTTYPE = 4");
                    if (dt.Select("FLDDOCUMENTTYPE = 4").Length > 0)
                    {
                        DataTable result2 = dr4.CopyToDataTable();

                        if (result2.Rows.Count > 0)
                        {

                            chkListMedical.DataSource = result2;
                            chkListMedical.DataTextField = "FLDDOCUMENTNAME";
                            chkListMedical.DataValueField = "FLDDOCUMENTID";
                            chkListMedical.DataBind();
                            int j;
                            j = 0;
                            CheckBoxList chk2 = (CheckBoxList)e.Item.FindControl("chkListMedical");
                            foreach (ListItem li in chk2.Items)
                            {
                                if (j <= result2.Rows.Count - 1)
                                {
                                    string slist = result2.Rows[j]["FLDSELECTTEDDOCUMENTID"].ToString();
                                    if (li.Value.Equals(slist))
                                    {
                                        li.Selected = true;
                                    }
                                    j++;
                                }
                            }
                        }

                    }
                }

            }

        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
}
