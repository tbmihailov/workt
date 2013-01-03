using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WorkT.CodeGeneration.Model
{
        class Track
        {
            private int _trackId;
            public int TrackId
            {
                get { return _trackId; }
                set { _trackId = value; }
            }

            private int _projectId;
            public int ProjectId
            {
                get { return _projectId; }
                set { _projectId = value; }
            }

            private double _duration;
            public double Duration
            {
                get { return _duration; }
                set { _duration = value; }
            }

            private string _notes;
            public string Notes
            {
                get { return _notes; }
                set { _notes = value; }
            }

            private DateTime _fromDate;
            public DateTime FromDate
            {
                get { return _fromDate; }
                set { _fromDate = value; }
            }

            private DateTime _toDate;
            public DateTime ToDate
            {
                get { return _toDate; }
                set { _toDate = value; }
            }

            private DateTime _created;
            public DateTime Created
            {
                get { return _created; }
                set { _created = value; }
            }

        }
}
