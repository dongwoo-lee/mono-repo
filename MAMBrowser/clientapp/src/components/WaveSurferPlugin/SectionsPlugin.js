/**
 * @typedef {Object} SectionParams
 * @property {number} startTime The time to set the section at
 * @property {number} endTime The time to set the section at
 * @property {?label} string An optional section label
 * @property {?color} string Background color for section
 */
 
/**
 * Sections are points in time in the audio that can be jumped to.
 *
 * @implements {PluginClass}
 *
 * @example
 * import SectionsPlugin from 'wavesurfer.sections.js';
 *
 * // if you are using <script> tags
 * var SectionPlugin = window.WaveSurfer.sections;
 *
 * // ... initialising wavesurfer with the plugin
 * var wavesurfer = WaveSurfer.create({
 *   // wavesurfer options ...
 *   plugins: [
 *     SectionsPlugin.create({
 *       // plugin options ...
 *     })
 *   ]
 * });
 */
 
 export default class SectionsPlugin {
    /**
     * @typedef {Object} SectionsPluginParams
     * @property {?SectionParams[]} sections Initial set of sections
     * @fires SectionsPlugin#section-click
     */
 
    /**
     * Sections plugin definition factory
     *
     * This function must be used to create a plugin definition which can be
     * used by wavesurfer to correctly instantiate the plugin.
     *
     * @param {SectionsPluginParams} params parameters use to initialise the plugin
     * @since 4.6.0
     * @return {PluginDefinition} an object representing the plugin
     */
    static create(params) {
        return {
            name: 'sections',
            deferInit: params && params.deferInit ? params.deferInit : false,
            params: params,
            staticProps: {
                addSection(options) {
                    console.info('options', options);
                    if (!this.initialisedPluginList.sections) {
                        this.initPlugin('sections');
                    }
                    console.info('this', this);
                    return this.sections.add(options);
                },
                clearSections() {
                    this.sections && this.sections.clear();
                },
            },
            instance: SectionsPlugin
        };
    }
 
    constructor(params, ws) {
        console.info('cont params',params);
        console.info('cont ws',ws);
        this.params = params;
        this.wavesurfer = ws;
        this.util = ws.util;
        this.style = this.util.style;
        this.container = document.createElement('section-container');
        const wrapper = document.createElement('section');
        wrapper.className = "wavesurfer-section";
        this.wrapper = wrapper;
        this.container.insertBefore(wrapper, this.container.firstChild);
        this.wavesurfer.container.insertBefore(this.container, this.wavesurfer.container.firstChild);

        this.style(wrapper, {
           position:"relative",
           display:"block",
           height:"25px",
           // overflow:"hidden",
        });

        this.style(this.container, {
           // position:"relative",
           // display:"block",
           height:"25px",
           display : "block",
           overflow:"hidden",
        });
        this._onResize = () => {
            this._updateSectionPositions();
        };
 
        this._onBackendCreated = () => {
            if (this.params.sections) {
                this.params.sections.forEach(section => this.add(section));
            }
            window.addEventListener('resize', this._onResize, true);
            window.addEventListener('orientationchange', this._onResize, true);
            this.wavesurfer.on('zoom', this._onResize);
        };
 
        this.sections = [];
        this._onReady = () => {
            ws.drawer.wrapper.addEventListener('scroll', this._onScroll);
            this._updateSectionPositions();
        };
    }
    _onScroll = () => {
       if (this.container) {
           this.container.scrollLeft = this.wavesurfer.drawer.wrapper.scrollLeft;
       }
   };
    init() {
        // Check if ws is ready
        if (this.wavesurfer.isReady) {
            this._onBackendCreated();
            this._onReady();
        } else {
            this.wavesurfer.once('ready', this._onReady);
            this.wavesurfer.once('backend-created', this._onBackendCreated);
        }
    }
 
    destroy() {
        this.wavesurfer.un('ready', this._onReady);
        this.wavesurfer.un('backend-created', this._onBackendCreated);
 
        this.wavesurfer.un('zoom', this._onResize);
 
        window.removeEventListener('resize', this._onResize, true);
        window.removeEventListener('orientationchange', this._onResize, true);
 
        this.clear();
    }
 
    /**
     * Add a section
     *
     * @param {SectionParams} params Section definition
     * @return {object} The created section
     */
    add(params) {
        console.info('params',params);
        let section = {
            startTime: params.startTime,
            endTime: params.endTime,
            label: params.label,
            color: params.color,
        };
 
        section.el = this._createSectionElement(section);
        this.wrapper.insertBefore(section.el, this.wrapper.firstChild);
        this.sections.push(section);
        this._updateSectionPositions();
 
        return section;
    }
 
    /**
     * Remove a section
     *
     * @param {number} index Index of the section to remove
     */
    remove(index) {
        let section = this.sections[index];
        if (!section) {
            return;
        }
 
        this.wrapper.removeChild(section.el);
        this.sections.splice(index, 1);
    }
 
    _createSectionElement(section) {
        const el = document.createElement('div');
        this.style(el, {
            "display":"flex",
            "position":"absolute",
            "background-color": "rgba(255,255,0,0.15)", 
            width: "1200px",
            "z-index":6,       
            height:"25px",
            "color" : "black",
            border : "1px solid"
        });

        const textEle = document.createElement('div');
        textEle.textContent = section.label;
        textEle.title = section.label;
        this.style(textEle, {
           "position":"sticky",
           left:"15px",
           overflow:"hidden",
           "text-overflow":"ellipsis",
           "white-space":"nowrap",
           cursor : "default"
       });

        el.insertBefore(textEle, el.firstChild);
        return el;
    }
    getWidth() {
       return this.wavesurfer.drawer.width / this.wavesurfer.params.pixelRatio;
    }
    _updateSectionPositions() {
       const dur = this.wavesurfer.getDuration();
       const width = this.getWidth();
       

       for ( let i = 0 ; i < this.sections.length; i++ ) {
           let startLimited = this.sections[i].startTime;
           let endLimited = this.sections[i].endTime;
           if (startLimited < 0) {
               startLimited = 0;
               endLimited = endLimited - startLimited;
           }
           if (endLimited > dur) {
               endLimited = dur;
               startLimited = dur - (endLimited - startLimited);
           }
           if (this.sections[i].el != null) {
               // Calculate the left and width values of the region such that
               // no gaps appear between regions.
               const left = Math.round((startLimited / dur) * width);
               const regionWidth = Math.round((endLimited / dur) * width) - left;

               this.style(this.sections[i].el, {
                   left: left + 'px',
                   width: regionWidth + 'px',
                   // backgroundColor: this.sections[i].color,
               });

               this.style(this.wrapper, {
                   width : this.wavesurfer.drawer.width + 'px'
                });
           }
       }
    }
 
    /**
     * Remove all sections
     */
    clear() {
        while ( this.sections.length > 0 ) {
            this.remove(0);
        }
    }

}