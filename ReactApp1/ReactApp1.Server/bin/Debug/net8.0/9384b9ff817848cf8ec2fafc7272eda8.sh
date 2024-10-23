function list_child_processes () {
    local ppid=$1;
    local current_children=$(pgrep -P $ppid);
    local local_child;
    if [ $? -eq 0 ];
    then
        for current_child in $current_children
        do
          local_child=$current_child;
          list_child_processes $local_child;
          echo $local_child;
        done;
    else
      return 0;
    fi;
}

ps 33711;
while [ $? -eq 0 ];
do
  sleep 1;
  ps 33711 > /dev/null;
done;

for child in $(list_child_processes 33723);
do
  echo killing $child;
  kill -s KILL $child;
done;
rm /Users/dominykas.cernovas/Documents/Documents - Dominykas’s Laptop - 1/PSP/PSP2/ReactApp1/ReactApp1.Server/bin/Debug/net8.0/9384b9ff817848cf8ec2fafc7272eda8.sh;
