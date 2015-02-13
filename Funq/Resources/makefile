all: \
    $(patsubst %.strings,%.resources,$(wildcard *.strings))

%.resources: %.strings
	/usr/local/bin/strapper $< -i -n:Funq
	