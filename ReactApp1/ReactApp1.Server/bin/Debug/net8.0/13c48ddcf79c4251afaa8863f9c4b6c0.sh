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

ps 34517;
while [ $? -eq 0 ];
do
  sleep 1;
  ps 34517 > /dev/null;
done;

for child in $(list_child_processes 34566);
do
  echo killing $child;
  kill -s KILL $child;
done;
rm /Users/dominykas.cernovas/Documents/Documents - Dominykas’s Laptop - 1/PSP/PSP2/ReactApp1/ReactApp1.Server/bin/Debug/net8.0/13c48ddcf79c4251afaa8863f9c4b6c0.sh;
