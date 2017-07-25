/*!
 * ASP.NET SignalR JavaScript Library v1.1.3
 * http://signalr.net/
 *
 * Copyright Microsoft Open Technologies, Inc. All rights reserved.
 * Licensed under the Apache 2.0
 * https://github.com/SignalR/SignalR/blob/master/LICENSE.md
 *
 */

/// <reference path="..\..\SignalR.Client.JS\Scripts\jquery-1.6.4.js" />
/// <reference path="jquery.signalR.js" />
(function ($, window) {
    /// <param name="$" type="jQuery" />
    "use strict";

    if (typeof ($.signalR) !== "function") {
        throw new Error("SignalR: SignalR is not loaded. Please ensure jquery.signalR-x.js is referenced before ~/signalr/hubs.");
    }

    var signalR = $.signalR;

    function makeProxyCallback(hub, callback) {
        return function () {
            // Call the client hub method
            callback.apply(hub, $.makeArray(arguments));
        };
    }

    function registerHubProxies(instance, shouldSubscribe) {
        var key, hub, memberKey, memberValue, subscriptionMethod;

        for (key in instance) {
            if (instance.hasOwnProperty(key)) {
                hub = instance[key];

                if (!(hub.hubName)) {
                    // Not a client hub
                    continue;
                }

                if (shouldSubscribe) {
                    // We want to subscribe to the hub events
                    subscriptionMethod = hub.on;
                }
                else {
                    // We want to unsubscribe from the hub events
                    subscriptionMethod = hub.off;
                }

                // Loop through all members on the hub and find client hub functions to subscribe/unsubscribe
                for (memberKey in hub.client) {
                    if (hub.client.hasOwnProperty(memberKey)) {
                        memberValue = hub.client[memberKey];

                        if (!$.isFunction(memberValue)) {
                            // Not a client hub function
                            continue;
                        }

                        subscriptionMethod.call(hub, memberKey, makeProxyCallback(hub, memberValue));
                    }
                }
            }
        }
    }

    $.hubConnection.prototype.createHubProxies = function () {
        var proxies = {};
        this.starting(function () {
            // Register the hub proxies as subscribed
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, true);

            this._registerSubscribedHubs();
        }).disconnected(function () {
            // Unsubscribe all hub proxies when we "disconnect".  This is to ensure that we do not re-add functional call backs.
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, false);
        });

        proxies.adminAuthHub = this.createHubProxy('adminAuthHub'); 
        proxies.adminAuthHub.client = { };
        proxies.adminAuthHub.server = {
            invokedFromClient: function () {
                return proxies.adminAuthHub.invoke.apply(proxies.adminAuthHub, $.merge(["InvokedFromClient"], $.makeArray(arguments)));
             }
        };

        proxies.authHub = this.createHubProxy('authHub'); 
        proxies.authHub.client = { };
        proxies.authHub.server = {
            invokedFromClient: function () {
                return proxies.authHub.invoke.apply(proxies.authHub, $.merge(["InvokedFromClient"], $.makeArray(arguments)));
             }
        };

        proxies.chat = this.createHubProxy('chat'); 
        proxies.chat.client = { };
        proxies.chat.server = {
            getUsers: function () {
                return proxies.chat.invoke.apply(proxies.chat, $.merge(["GetUsers"], $.makeArray(arguments)));
             },

            join: function () {
                return proxies.chat.invoke.apply(proxies.chat, $.merge(["Join"], $.makeArray(arguments)));
             },

            send: function (content) {
                return proxies.chat.invoke.apply(proxies.chat, $.merge(["Send"], $.makeArray(arguments)));
             }
        };

        proxies.countingHub = this.createHubProxy('countingHub'); 
        proxies.countingHub.client = { };
        proxies.countingHub.server = {
            send: function (n) {
                return proxies.countingHub.invoke.apply(proxies.countingHub, $.merge(["Send"], $.makeArray(arguments)));
             }
        };

        proxies.demo = this.createHubProxy('demo'); 
        proxies.demo.client = { };
        proxies.demo.server = {
            addToGroups: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["AddToGroups"], $.makeArray(arguments)));
             },

            cancelledGenericTask: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["CancelledGenericTask"], $.makeArray(arguments)));
             },

            cancelledTask: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["CancelledTask"], $.makeArray(arguments)));
             },

            complexArray: function (people) {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["ComplexArray"], $.makeArray(arguments)));
             },

            complexType: function (p) {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["ComplexType"], $.makeArray(arguments)));
             },

            doSomethingAndCallError: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["DoSomethingAndCallError"], $.makeArray(arguments)));
             },

            dynamicInvoke: function (method) {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["DynamicInvoke"], $.makeArray(arguments)));
             },

            dynamicTask: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["DynamicTask"], $.makeArray(arguments)));
             },

            genericTaskWithContinueWith: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["GenericTaskWithContinueWith"], $.makeArray(arguments)));
             },

            genericTaskWithException: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["GenericTaskWithException"], $.makeArray(arguments)));
             },

            getValue: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["GetValue"], $.makeArray(arguments)));
             },

            inlineScriptTag: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["InlineScriptTag"], $.makeArray(arguments)));
             },

            mispelledClientMethod: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["MispelledClientMethod"], $.makeArray(arguments)));
             },

            multipleCalls: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["MultipleCalls"], $.makeArray(arguments)));
             },

            neverEndingTask: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["NeverEndingTask"], $.makeArray(arguments)));
             },

            overload: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["Overload"], $.makeArray(arguments)));
             },

            passingDynamicComplex: function (p) {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["PassingDynamicComplex"], $.makeArray(arguments)));
             },

            plainTask: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["PlainTask"], $.makeArray(arguments)));
             },

            readAnyState: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["ReadAnyState"], $.makeArray(arguments)));
             },

            readStateValue: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["ReadStateValue"], $.makeArray(arguments)));
             },

            setStateValue: function (value) {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["SetStateValue"], $.makeArray(arguments)));
             },

            simpleArray: function (nums) {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["SimpleArray"], $.makeArray(arguments)));
             },

            synchronousException: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["SynchronousException"], $.makeArray(arguments)));
             },

            taskWithException: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["TaskWithException"], $.makeArray(arguments)));
             },

            testGuid: function () {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["TestGuid"], $.makeArray(arguments)));
             },

            unsupportedOverload: function (x) {
                return proxies.demo.invoke.apply(proxies.demo, $.merge(["UnsupportedOverload"], $.makeArray(arguments)));
             }
        };

        proxies.DrawingPad = this.createHubProxy('DrawingPad'); 
        proxies.DrawingPad.client = { };
        proxies.DrawingPad.server = {
            Draw: function (data) {
                return proxies.DrawingPad.invoke.apply(proxies.DrawingPad, $.merge(["Draw"], $.makeArray(arguments)));
             },

            join: function () {
                return proxies.DrawingPad.invoke.apply(proxies.DrawingPad, $.merge(["Join"], $.makeArray(arguments)));
             }
        };

        proxies.EventHub = this.createHubProxy('EventHub'); 
        proxies.EventHub.client = { };
        proxies.EventHub.server = {
            displayMessage: function (eventstr) {
                return proxies.EventHub.invoke.apply(proxies.EventHub, $.merge(["displayMessage"], $.makeArray(arguments)));
             }
        };

        proxies.headerAuthHub = this.createHubProxy('headerAuthHub'); 
        proxies.headerAuthHub.client = { };
        proxies.headerAuthHub.server = {
        };

        proxies.hubBench = this.createHubProxy('hubBench'); 
        proxies.hubBench.client = { };
        proxies.hubBench.server = {
            hitMe: function (start, clientCalls, connectionId) {
                return proxies.hubBench.invoke.apply(proxies.hubBench, $.merge(["HitMe"], $.makeArray(arguments)));
             },

            hitUs: function (start, clientCalls) {
                return proxies.hubBench.invoke.apply(proxies.hubBench, $.merge(["HitUs"], $.makeArray(arguments)));
             }
        };

        proxies.hubConnectionAPI = this.createHubProxy('hubConnectionAPI'); 
        proxies.hubConnectionAPI.client = { };
        proxies.hubConnectionAPI.server = {
            displayMessageAll: function (message) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["DisplayMessageAll"], $.makeArray(arguments)));
             },

            displayMessageAllExcept: function (message, excludeConnectionIds) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["DisplayMessageAllExcept"], $.makeArray(arguments)));
             },

            displayMessageCaller: function (message) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["DisplayMessageCaller"], $.makeArray(arguments)));
             },

            displayMessageGroup: function (groupName, message) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["DisplayMessageGroup"], $.makeArray(arguments)));
             },

            displayMessageGroupExcept: function (groupName, message, excludeConnectionIds) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["DisplayMessageGroupExcept"], $.makeArray(arguments)));
             },

            displayMessageOther: function (message) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["DisplayMessageOther"], $.makeArray(arguments)));
             },

            displayMessageOthersInGroup: function (groupName, message) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["DisplayMessageOthersInGroup"], $.makeArray(arguments)));
             },

            displayMessageSpecified: function (targetConnectionId, message) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["DisplayMessageSpecified"], $.makeArray(arguments)));
             },

            joinGroup: function (connectionId, groupName) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["JoinGroup"], $.makeArray(arguments)));
             },

            leaveGroup: function (connectionId, groupName) {
                return proxies.hubConnectionAPI.invoke.apply(proxies.hubConnectionAPI, $.merge(["LeaveGroup"], $.makeArray(arguments)));
             }
        };

        proxies.incomingAuthHub = this.createHubProxy('incomingAuthHub'); 
        proxies.incomingAuthHub.client = { };
        proxies.incomingAuthHub.server = {
            invokedFromClient: function () {
                return proxies.incomingAuthHub.invoke.apply(proxies.incomingAuthHub, $.merge(["InvokedFromClient"], $.makeArray(arguments)));
             }
        };

        proxies.inheritAuthHub = this.createHubProxy('inheritAuthHub'); 
        proxies.inheritAuthHub.client = { };
        proxies.inheritAuthHub.server = {
            invokedFromClient: function () {
                return proxies.inheritAuthHub.invoke.apply(proxies.inheritAuthHub, $.merge(["InvokedFromClient"], $.makeArray(arguments)));
             }
        };

        proxies.invokeAuthHub = this.createHubProxy('invokeAuthHub'); 
        proxies.invokeAuthHub.client = { };
        proxies.invokeAuthHub.server = {
            invokedFromClient: function () {
                return proxies.invokeAuthHub.invoke.apply(proxies.invokeAuthHub, $.merge(["InvokedFromClient"], $.makeArray(arguments)));
             }
        };

        proxies.messageLoops = this.createHubProxy('messageLoops'); 
        proxies.messageLoops.client = { };
        proxies.messageLoops.server = {
            joinGroup: function (connectionId, groupName) {
                return proxies.messageLoops.invoke.apply(proxies.messageLoops, $.merge(["JoinGroup"], $.makeArray(arguments)));
             },

            leaveGroup: function (connectionId, groupName) {
                return proxies.messageLoops.invoke.apply(proxies.messageLoops, $.merge(["LeaveGroup"], $.makeArray(arguments)));
             },

            sendMessageCountToAll: function (messageCount, sleepTime) {
                return proxies.messageLoops.invoke.apply(proxies.messageLoops, $.merge(["SendMessageCountToAll"], $.makeArray(arguments)));
             },

            sendMessageCountToCaller: function (messageCount, sleepTime) {
                return proxies.messageLoops.invoke.apply(proxies.messageLoops, $.merge(["SendMessageCountToCaller"], $.makeArray(arguments)));
             },

            sendMessageCountToGroup: function (messageCount, groupName, sleepTime) {
                return proxies.messageLoops.invoke.apply(proxies.messageLoops, $.merge(["SendMessageCountToGroup"], $.makeArray(arguments)));
             }
        };

        proxies.mouseTracking = this.createHubProxy('mouseTracking'); 
        proxies.mouseTracking.client = { };
        proxies.mouseTracking.server = {
            join: function () {
                return proxies.mouseTracking.invoke.apply(proxies.mouseTracking, $.merge(["Join"], $.makeArray(arguments)));
             },

            move: function (x, y) {
                return proxies.mouseTracking.invoke.apply(proxies.mouseTracking, $.merge(["Move"], $.makeArray(arguments)));
             }
        };

        proxies.noAuthHub = this.createHubProxy('noAuthHub'); 
        proxies.noAuthHub.client = { };
        proxies.noAuthHub.server = {
            invokedFromClient: function () {
                return proxies.noAuthHub.invoke.apply(proxies.noAuthHub, $.merge(["InvokedFromClient"], $.makeArray(arguments)));
             }
        };

        proxies.realtime = this.createHubProxy('realtime'); 
        proxies.realtime.client = { };
        proxies.realtime.server = {
            getFPS: function () {
                return proxies.realtime.invoke.apply(proxies.realtime, $.merge(["GetFPS"], $.makeArray(arguments)));
             },

            getFrameId: function () {
                return proxies.realtime.invoke.apply(proxies.realtime, $.merge(["GetFrameId"], $.makeArray(arguments)));
             },

            isEngineRunning: function () {
                return proxies.realtime.invoke.apply(proxies.realtime, $.merge(["IsEngineRunning"], $.makeArray(arguments)));
             },

            setFPS: function (fps) {
                return proxies.realtime.invoke.apply(proxies.realtime, $.merge(["SetFPS"], $.makeArray(arguments)));
             },

            start: function () {
                return proxies.realtime.invoke.apply(proxies.realtime, $.merge(["Start"], $.makeArray(arguments)));
             },

            stop: function () {
                return proxies.realtime.invoke.apply(proxies.realtime, $.merge(["Stop"], $.makeArray(arguments)));
             }
        };

        proxies.shapeShare = this.createHubProxy('shapeShare'); 
        proxies.shapeShare.client = { };
        proxies.shapeShare.server = {
            changeShape: function (id, x, y, w, h) {
                return proxies.shapeShare.invoke.apply(proxies.shapeShare, $.merge(["ChangeShape"], $.makeArray(arguments)));
             },

            changeUserName: function (currentUserName, newUserName) {
                return proxies.shapeShare.invoke.apply(proxies.shapeShare, $.merge(["ChangeUserName"], $.makeArray(arguments)));
             },

            createShape: function (type) {
                return proxies.shapeShare.invoke.apply(proxies.shapeShare, $.merge(["CreateShape"], $.makeArray(arguments)));
             },

            deleteAllShapes: function () {
                return proxies.shapeShare.invoke.apply(proxies.shapeShare, $.merge(["DeleteAllShapes"], $.makeArray(arguments)));
             },

            deleteShape: function (id) {
                return proxies.shapeShare.invoke.apply(proxies.shapeShare, $.merge(["DeleteShape"], $.makeArray(arguments)));
             },

            getShapes: function () {
                return proxies.shapeShare.invoke.apply(proxies.shapeShare, $.merge(["GetShapes"], $.makeArray(arguments)));
             },

            join: function (userName) {
                return proxies.shapeShare.invoke.apply(proxies.shapeShare, $.merge(["Join"], $.makeArray(arguments)));
             }
        };

        proxies.StatusHub = this.createHubProxy('StatusHub'); 
        proxies.StatusHub.client = { };
        proxies.StatusHub.server = {
            ping: function () {
                return proxies.StatusHub.invoke.apply(proxies.StatusHub, $.merge(["Ping"], $.makeArray(arguments)));
             }
        };

        proxies.SvgHub = this.createHubProxy('SvgHub'); 
        proxies.SvgHub.client = { };
        proxies.SvgHub.server = {
            updateStatus: function (svgid, shapeguid, svgshapename, svalue, mappingtabletype) {
                return proxies.SvgHub.invoke.apply(proxies.SvgHub, $.merge(["UpdateStatus"], $.makeArray(arguments)));
             }
        };

        proxies.userAndRoleAuthHub = this.createHubProxy('userAndRoleAuthHub'); 
        proxies.userAndRoleAuthHub.client = { };
        proxies.userAndRoleAuthHub.server = {
            invokedFromClient: function () {
                return proxies.userAndRoleAuthHub.invoke.apply(proxies.userAndRoleAuthHub, $.merge(["InvokedFromClient"], $.makeArray(arguments)));
             }
        };

        return proxies;
    };

    signalR.hub = $.hubConnection("/signalr", { useDefaultPath: false });
    $.extend(signalR, signalR.hub.createHubProxies());

}(window.jQuery, window));