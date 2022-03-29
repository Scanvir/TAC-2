using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAC_2
{
    public class ListExtendableGoodAdapter : BaseExpandableListAdapter
    {
        private Context context;
        private List<string> listGroup;
        private Dictionary<string, List<string>> listChild;

        public ListExtendableGoodAdapter(Context context, List<string> listGroup, Dictionary<string, List<string>> listChild)
        {
            this.context = context;
            this.listGroup = listGroup;
            this.listChild = listChild; 
        }

        public override int GroupCount => listGroup.Count;

        public override bool HasStableIds => false;

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            var result = new List<string>();
            listChild.TryGetValue(listGroup[groupPosition], out result);
            return result[childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            var result = new List<string>();
            listChild.TryGetValue(listGroup[groupPosition], out result);
            return result.Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            if(convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.good_item, null);
            }
            TextView param1_1 = convertView.FindViewById<TextView>(Resource.Id.Param1_1);
            string content = (string)GetChild(groupPosition, childPosition);
            param1_1.Text = content;
            return convertView;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return listGroup[groupPosition];
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                LayoutInflater inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.good_group, null);
            }
            TextView Name = convertView.FindViewById<TextView>(Resource.Id.Name);
            string groupName = (string)GetGroup(groupPosition);
            Name.Text = groupName;
            return convertView;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}